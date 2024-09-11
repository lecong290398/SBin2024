using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.ProductTypes;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.ProductTypes;
using DTKH2024.SbinSolution.ProductTypes.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductTypes)]
    public class ProductTypesController : SbinSolutionControllerBase
    {
        private readonly IProductTypesAppService _productTypesAppService;

        public ProductTypesController(IProductTypesAppService productTypesAppService)
        {
            _productTypesAppService = productTypesAppService;

        }

        public ActionResult Index()
        {
            var model = new ProductTypesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_ProductTypes_Create, AppPermissions.Pages_Administration_ProductTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetProductTypeForEditOutput getProductTypeForEditOutput;

            if (id.HasValue)
            {
                getProductTypeForEditOutput = await _productTypesAppService.GetProductTypeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getProductTypeForEditOutput = new GetProductTypeForEditOutput
                {
                    ProductType = new CreateOrEditProductTypeDto()
                };
            }

            var viewModel = new CreateOrEditProductTypeModalViewModel()
            {
                ProductType = getProductTypeForEditOutput.ProductType,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewProductTypeModal(int id)
        {
            var getProductTypeForViewDto = await _productTypesAppService.GetProductTypeForView(id);

            var model = new ProductTypeViewModel()
            {
                ProductType = getProductTypeForViewDto.ProductType
            };

            return PartialView("_ViewProductTypeModal", model);
        }

    }
}