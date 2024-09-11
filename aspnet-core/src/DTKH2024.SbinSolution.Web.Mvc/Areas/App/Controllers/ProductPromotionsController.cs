using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.ProductPromotions;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.ProductPromotions;
using DTKH2024.SbinSolution.ProductPromotions.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductPromotions)]
    public class ProductPromotionsController : SbinSolutionControllerBase
    {
        private readonly IProductPromotionsAppService _productPromotionsAppService;

        public ProductPromotionsController(IProductPromotionsAppService productPromotionsAppService)
        {
            _productPromotionsAppService = productPromotionsAppService;

        }

        public ActionResult Index()
        {
            var model = new ProductPromotionsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductPromotions_Create, AppPermissions.Pages_Administration_ProductPromotions_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetProductPromotionForEditOutput getProductPromotionForEditOutput;

            if (id.HasValue)
            {
                getProductPromotionForEditOutput = await _productPromotionsAppService.GetProductPromotionForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getProductPromotionForEditOutput = new GetProductPromotionForEditOutput
                {
                    ProductPromotion = new CreateOrEditProductPromotionDto()
                };
                getProductPromotionForEditOutput.ProductPromotion.StartDate = DateTime.Now;
                getProductPromotionForEditOutput.ProductPromotion.EndDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditProductPromotionModalViewModel()
            {
                ProductPromotion = getProductPromotionForEditOutput.ProductPromotion,
                ProductProductName = getProductPromotionForEditOutput.ProductProductName,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductPromotionModal(int id)
        {
            var getProductPromotionForViewDto = await _productPromotionsAppService.GetProductPromotionForView(id);

            var model = new ProductPromotionViewModel()
            {
                ProductPromotion = getProductPromotionForViewDto.ProductPromotion
                ,
                ProductProductName = getProductPromotionForViewDto.ProductProductName

            };

            return PartialView("_ViewProductPromotionModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductPromotions_Create, AppPermissions.Pages_Administration_ProductPromotions_Edit)]
        public PartialViewResult ProductLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ProductPromotionProductLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ProductPromotionProductLookupTableModal", viewModel);
        }

    }
}