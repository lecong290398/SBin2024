using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.Layout;
using DTKH2024.SbinSolution.Web.Views;

namespace DTKH2024.SbinSolution.Web.Areas.App.Views.Shared.Components.AppToggleDarkMode
{
    public class AppToggleDarkModeViewComponent : SbinSolutionViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass, bool isDarkModeActive)
        {
            return Task.FromResult<IViewComponentResult>(View(new ToggleDarkModeViewModel(cssClass, isDarkModeActive)));
        }
    }
}