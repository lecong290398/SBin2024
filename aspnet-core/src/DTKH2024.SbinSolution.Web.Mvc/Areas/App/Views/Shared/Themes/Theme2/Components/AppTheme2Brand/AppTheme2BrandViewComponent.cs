using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.Layout;
using DTKH2024.SbinSolution.Web.Session;
using DTKH2024.SbinSolution.Web.Views;

namespace DTKH2024.SbinSolution.Web.Areas.App.Views.Shared.Themes.Theme2.Components.AppTheme2Brand
{
    public class AppTheme2BrandViewComponent : SbinSolutionViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme2BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
