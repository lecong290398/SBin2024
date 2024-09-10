using Abp.Localization;
using Abp.Web.Models.AbpUserConfiguration;
using JetBrains.Annotations;
using DTKH2024.SbinSolution.Sessions.Dto;

namespace DTKH2024.SbinSolution.ApiClient
{
    public interface IApplicationContext
    {
        [CanBeNull]
        TenantInformation CurrentTenant { get; }

        AbpUserConfigurationDto Configuration { get; set; }

        GetCurrentLoginInformationsOutput LoginInfo { get; }

        void ClearLoginInfo();

        void SetLoginInfo(GetCurrentLoginInformationsOutput loginInfo);

        void SetAsHost();

        void SetAsTenant(string tenancyName, int tenantId);

        LanguageInfo CurrentLanguage { get; set; }

        void Load(TenantInformation currentTenant, GetCurrentLoginInformationsOutput loginInfo);
    }
}