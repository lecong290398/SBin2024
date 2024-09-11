using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.CategoryPromotions;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.CategoryPromotions;
using DTKH2024.SbinSolution.CategoryPromotions.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_CategoryPromotions)]
    public class CategoryPromotionsController : SbinSolutionControllerBase
    {
        private readonly ICategoryPromotionsAppService _categoryPromotionsAppService;

        public CategoryPromotionsController(ICategoryPromotionsAppService categoryPromotionsAppService)
        {
            _categoryPromotionsAppService = categoryPromotionsAppService;

        }

        public ActionResult Index()
        {
            var model = new CategoryPromotionsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_CategoryPromotions_Create, AppPermissions.Pages_Administration_CategoryPromotions_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCategoryPromotionForEditOutput getCategoryPromotionForEditOutput;

            if (id.HasValue)
            {
                getCategoryPromotionForEditOutput = await _categoryPromotionsAppService.GetCategoryPromotionForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getCategoryPromotionForEditOutput = new GetCategoryPromotionForEditOutput
                {
                    CategoryPromotion = new CreateOrEditCategoryPromotionDto()
                };
            }

            var viewModel = new CreateOrEditCategoryPromotionModalViewModel()
            {
                CategoryPromotion = getCategoryPromotionForEditOutput.CategoryPromotion,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCategoryPromotionModal(int id)
        {
            var getCategoryPromotionForViewDto = await _categoryPromotionsAppService.GetCategoryPromotionForView(id);

            var model = new CategoryPromotionViewModel()
            {
                CategoryPromotion = getCategoryPromotionForViewDto.CategoryPromotion
            };

            return PartialView("_ViewCategoryPromotionModal", model);
        }

    }
}