﻿using System.Threading.Tasks;
using Abp.Application.Services;
using DTKH2024.SbinSolution.Configuration.Tenants.Dto;

namespace DTKH2024.SbinSolution.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearDarkLogo();
        
        Task ClearDarkLogoMinimal();

        Task ClearLightLogo();
        
        Task ClearLightLogoMinimal();

        Task ClearCustomCss();
    }
}
