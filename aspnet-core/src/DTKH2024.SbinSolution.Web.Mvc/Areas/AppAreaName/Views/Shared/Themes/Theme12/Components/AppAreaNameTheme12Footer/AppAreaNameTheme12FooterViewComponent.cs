﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Layout;
using DTKH2024.SbinSolution.Web.Session;
using DTKH2024.SbinSolution.Web.Views;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Views.Shared.Themes.Theme12.Components.AppAreaNameTheme12Footer
{
    public class AppAreaNameTheme12FooterViewComponent : SbinSolutionViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppAreaNameTheme12FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
