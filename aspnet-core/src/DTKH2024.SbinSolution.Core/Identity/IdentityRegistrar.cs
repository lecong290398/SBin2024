using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using DTKH2024.SbinSolution.Authentication.TwoFactor.Google;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.Authorization.Roles;
using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.Editions;
using DTKH2024.SbinSolution.MultiTenancy;

namespace DTKH2024.SbinSolution.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>(options =>
                {
                    options.Tokens.ProviderMap[GoogleAuthenticatorProvider.Name] = new TokenProviderDescriptor(typeof(GoogleAuthenticatorProvider));
                })
                .AddAbpTenantManager<TenantManager>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddPermissionChecker<PermissionChecker>()
                .AddDefaultTokenProviders();
        }
    }
}
