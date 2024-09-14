using Abp.AspNetCore.Mvc.Authorization;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RedeemGifts)]
    public class RedeemGiftsController : SbinSolutionControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
