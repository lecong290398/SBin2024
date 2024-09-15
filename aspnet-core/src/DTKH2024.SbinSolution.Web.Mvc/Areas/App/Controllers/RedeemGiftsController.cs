using Abp.AspNetCore.Mvc.Authorization;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.Brands.Dtos;
using DTKH2024.SbinSolution.Brands.Exporting;
using DTKH2024.SbinSolution.RedeemGifts;
using DTKH2024.SbinSolution.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_RedeemGifts)]
    public class RedeemGiftsController : SbinSolutionControllerBase
    {
        private readonly IRedeemGiftsAppService  _redeemGiftsAppService;
        public RedeemGiftsController(IRedeemGiftsAppService redeemGiftsAppService)
        {
            _redeemGiftsAppService = redeemGiftsAppService;
        }
        public IActionResult Index()
        {
            var input = new GetAllBrandsInput
            {
                SkipCount = 0,
                MaxResultCount = 100
            };
            var brandResult = _redeemGiftsAppService.GetAllBrand(input).Result;
            ViewBag.Brands = brandResult.Items;
            return View();
        }
    }
}
