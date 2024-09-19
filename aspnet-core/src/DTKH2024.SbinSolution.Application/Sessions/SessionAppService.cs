using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using DTKH2024.SbinSolution.Authentication.TwoFactor;
using DTKH2024.SbinSolution.Editions;
using DTKH2024.SbinSolution.MultiTenancy.Payments;
using DTKH2024.SbinSolution.Sessions.Dto;
using DTKH2024.SbinSolution.UiCustomization;
using DTKH2024.SbinSolution.Authorization.Delegation;
using DTKH2024.SbinSolution.Authorization.Users;
using Abp.Domain.Uow;
using Abp.Localization;
using DTKH2024.SbinSolution.Features;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using PayPalCheckoutSdk.Orders;
using DTKH2024.SbinSolution.RankLevels;

namespace DTKH2024.SbinSolution.Sessions
{
    public class SessionAppService : SbinSolutionAppServiceBase, ISessionAppService
    {
        private readonly IUiThemeCustomizerFactory _uiThemeCustomizerFactory;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserDelegationConfiguration _userDelegationConfiguration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly EditionManager _editionManager;
        private readonly ILocalizationContext _localizationContext;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<RankLevel> _rankLevelRepository;

        public SessionAppService(
            IUiThemeCustomizerFactory uiThemeCustomizerFactory,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IUserDelegationConfiguration userDelegationConfiguration,
            IUnitOfWorkManager unitOfWorkManager,
            EditionManager editionManager, ILocalizationContext localizationContext
            , IRepository<UserRole, long> userRoleRepository, IRepository<RankLevel> rankLevelRepository)
        {
            _uiThemeCustomizerFactory = uiThemeCustomizerFactory;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userDelegationConfiguration = userDelegationConfiguration;
            _unitOfWorkManager = unitOfWorkManager;
            _editionManager = editionManager;
            _localizationContext = localizationContext;
            _userRoleRepository = userRoleRepository;
            _rankLevelRepository = rankLevelRepository;
        }

        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            return await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                var output = new GetCurrentLoginInformationsOutput
                {
                    Application = new ApplicationInfoDto
                    {
                        Version = AppVersionHelper.Version,
                        ReleaseDate = AppVersionHelper.ReleaseDate,
                        Features = new Dictionary<string, bool>(),
                        Currency = SbinSolutionConsts.Currency,
                        CurrencySign = SbinSolutionConsts.CurrencySign,
                        AllowTenantsToChangeEmailSettings = SbinSolutionConsts.AllowTenantsToChangeEmailSettings,
                        UserDelegationIsEnabled = _userDelegationConfiguration.IsEnabled,
                        TwoFactorCodeExpireSeconds = TwoFactorCodeCacheItem.DefaultSlidingExpireTime.TotalSeconds
                    }
                };

                var uiCustomizer = await _uiThemeCustomizerFactory.GetCurrentUiCustomizer();
                output.Theme = await uiCustomizer.GetUiSettings();

                if (AbpSession.TenantId.HasValue)
                {
                    output.Tenant = await GetTenantLoginInfo(AbpSession.GetTenantId());
                }

                if (AbpSession.ImpersonatorTenantId.HasValue)
                {
                    output.ImpersonatorTenant = await GetTenantLoginInfo(AbpSession.ImpersonatorTenantId.Value);
                }

                if (AbpSession.UserId.HasValue)
                {
                    var userDto = await GetCurrentUserAsync();
                    output.User = ObjectMapper.Map<UserLoginInfoDto>(userDto);

                    try
                    {
                        if (userDto.Id != AppConsts.UserIdAdmin)
                        {
                            var userRoles = await _userRoleRepository.GetAll()
                            .Where(userRole => userDto.Id == userRole.UserId)
                            .Select(userRole => userRole).ToListAsync();
                            output.User.Roles = ObjectMapper.Map<List<UserRole>>(userRoles);
                            if (userRoles.Exists(c => c.RoleId == AppConsts.RoleIdUser))
                            {
                                var rank = _rankLevelRepository.GetAllList().Where(c => c.MinimumPositiveScore <= output.User.PositivePoint)
                                           .OrderByDescending(c => c.MinimumPositiveScore)
                                           .FirstOrDefault();
                                if (rank != null)
                                {
                                    output.User.RankName = rank.Name;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }

                if (AbpSession.ImpersonatorUserId.HasValue)
                {
                    output.ImpersonatorUser = ObjectMapper.Map<UserLoginInfoDto>(await GetImpersonatorUserAsync());
                }

                if (output.Tenant == null)
                {
                    return output;
                }

                if (output.Tenant.Edition != null)
                {
                    var lastPayment =
                        await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(output.Tenant.Id,
                            null, null);
                    if (lastPayment != null)
                    {
                        output.Tenant.Edition.IsHighestEdition = IsEditionHighest(output.Tenant.Edition.Id,
                            lastPayment.GetPaymentPeriodType());
                    }
                }

                output.Tenant.SubscriptionDateString = GetTenantSubscriptionDateString(output);
                output.Tenant.CreationTimeString = output.Tenant.CreationTime.ToString("d");

                return output;
            });
        }

        private async Task<TenantLoginInfoDto> GetTenantLoginInfo(int tenantId)
        {
            var tenant = await TenantManager.Tenants
                .Include(t => t.Edition)
                .FirstAsync(t => t.Id == AbpSession.GetTenantId());

            var tenantLoginInfo = ObjectMapper
                .Map<TenantLoginInfoDto>(tenant);

            if (!tenant.EditionId.HasValue)
            {
                return tenantLoginInfo;
            }
            
            var features = FeatureManager
                .GetAll()
                .Where(feature => (feature[FeatureMetadata.CustomFeatureKey] as FeatureMetadata)?.IsVisibleOnPricingTable ?? false);
            
            var featureDictionary = features.ToDictionary(feature => feature.Name, f => f);
            
            tenantLoginInfo.FeatureValues = (await _editionManager.GetFeatureValuesAsync(tenant.EditionId.Value))
                .Where(featureValue => featureDictionary.ContainsKey(featureValue.Name))
                .Select(fv => new NameValueDto(
                    featureDictionary[fv.Name].DisplayName.Localize(_localizationContext),
                    featureDictionary[fv.Name].GetValueText(fv.Value, _localizationContext))
                )
                .ToList();

            return tenantLoginInfo;
        }

        private bool IsEditionHighest(int editionId, PaymentPeriodType paymentPeriodType)
        {
            var topEdition = GetHighestEditionOrNullByPaymentPeriodType(paymentPeriodType);
            if (topEdition == null)
            {
                return false;
            }

            return editionId == topEdition.Id;
        }

        private SubscribableEdition GetHighestEditionOrNullByPaymentPeriodType(PaymentPeriodType paymentPeriodType)
        {
            var editions = TenantManager.EditionManager.Editions;
            if (editions == null || !editions.Any())
            {
                return null;
            }

            var query = editions.Cast<SubscribableEdition>();

            switch (paymentPeriodType)
            {
                case PaymentPeriodType.Monthly:
                    query = query.OrderByDescending(e => e.MonthlyPrice ?? 0);
                    break;
                case PaymentPeriodType.Annual:
                    query = query.OrderByDescending(e => e.AnnualPrice ?? 0);
                    break;
            }

            return query.FirstOrDefault();
        }

        private string GetTenantSubscriptionDateString(GetCurrentLoginInformationsOutput output)
        {
            return output.Tenant.SubscriptionEndDateUtc == null
                ? L("Unlimited")
                : output.Tenant.SubscriptionEndDateUtc?.ToString("d");
        }

        public async Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken()
        {
            if (AbpSession.UserId <= 0)
            {
                throw new Exception(L("ThereIsNoLoggedInUser"));
            }

            var user = await UserManager.GetUserAsync(AbpSession.ToUserIdentifier());
            user.SetSignInToken();
            return new UpdateUserSignInTokenOutput
            {
                SignInToken = user.SignInToken,
                EncodedUserId = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id.ToString())),
                EncodedTenantId = user.TenantId.HasValue
                    ? Convert.ToBase64String(Encoding.UTF8.GetBytes(user.TenantId.Value.ToString()))
                    : ""
            };
        }

        protected virtual async Task<User> GetImpersonatorUserAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.ImpersonatorTenantId))
            {
                var user = await UserManager.FindByIdAsync(AbpSession.ImpersonatorUserId.ToString());
                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                return user;
            }
        }
    }
}
