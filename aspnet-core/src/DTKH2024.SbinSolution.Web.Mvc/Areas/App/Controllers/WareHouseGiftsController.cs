using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.WareHouseGifts;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.WareHouseGifts;
using DTKH2024.SbinSolution.WareHouseGifts.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_WareHouseGifts)]
    public class WareHouseGiftsController : SbinSolutionControllerBase
    {
        private readonly IWareHouseGiftsAppService _wareHouseGiftsAppService;

        public WareHouseGiftsController(IWareHouseGiftsAppService wareHouseGiftsAppService)
        {
            _wareHouseGiftsAppService = wareHouseGiftsAppService;

        }

        public ActionResult Index()
        {
            var model = new WareHouseGiftsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_WareHouseGifts_Create, AppPermissions.Pages_WareHouseGifts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetWareHouseGiftForEditOutput getWareHouseGiftForEditOutput;

            if (id.HasValue)
            {
                getWareHouseGiftForEditOutput = await _wareHouseGiftsAppService.GetWareHouseGiftForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getWareHouseGiftForEditOutput = new GetWareHouseGiftForEditOutput
                {
                    WareHouseGift = new CreateOrEditWareHouseGiftDto()
                };
            }

            var viewModel = new CreateOrEditWareHouseGiftModalViewModel()
            {
                WareHouseGift = getWareHouseGiftForEditOutput.WareHouseGift,
                UserName = getWareHouseGiftForEditOutput.UserName,
                ProductPromotionPromotionCode = getWareHouseGiftForEditOutput.ProductPromotionPromotionCode,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewWareHouseGiftModal(int id)
        {
            var getWareHouseGiftForViewDto = await _wareHouseGiftsAppService.GetWareHouseGiftForView(id);

            var model = new WareHouseGiftViewModel()
            {
                WareHouseGift = getWareHouseGiftForViewDto.WareHouseGift
                ,
                UserName = getWareHouseGiftForViewDto.UserName

                ,
                ProductPromotionPromotionCode = getWareHouseGiftForViewDto.ProductPromotionPromotionCode

            };

            return PartialView("_ViewWareHouseGiftModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_WareHouseGifts_Create, AppPermissions.Pages_WareHouseGifts_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new WareHouseGiftUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_WareHouseGiftUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_WareHouseGifts_Create, AppPermissions.Pages_WareHouseGifts_Edit)]
        public PartialViewResult ProductPromotionLookupTableModal(int? id, string displayName)
        {
            var viewModel = new WareHouseGiftProductPromotionLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_WareHouseGiftProductPromotionLookupTableModal", viewModel);
        }

    }
}