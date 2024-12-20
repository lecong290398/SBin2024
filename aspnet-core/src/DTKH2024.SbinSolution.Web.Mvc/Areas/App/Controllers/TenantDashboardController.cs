﻿using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.DashboardCustomization;
using System.Threading.Tasks;
using DTKH2024.SbinSolution.Web.Areas.App.Startup;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardController : CustomizableDashboardControllerBase
    {
        public TenantDashboardController(DashboardViewConfiguration dashboardViewConfiguration, 
            IDashboardCustomizationAppService dashboardCustomizationAppService) 
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {

        }

        public async Task<ActionResult> Index()
        {
            return await GetView(SbinSolutionDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard);
        }
    }
}