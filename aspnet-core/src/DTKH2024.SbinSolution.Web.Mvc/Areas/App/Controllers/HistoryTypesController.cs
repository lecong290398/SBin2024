using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.HistoryTypes;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.HistoryTypes;
using DTKH2024.SbinSolution.HistoryTypes.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_HistoryTypes)]
    public class HistoryTypesController : SbinSolutionControllerBase
    {
        private readonly IHistoryTypesAppService _historyTypesAppService;

        public HistoryTypesController(IHistoryTypesAppService historyTypesAppService)
        {
            _historyTypesAppService = historyTypesAppService;

        }

        public ActionResult Index()
        {
            var model = new HistoryTypesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_HistoryTypes_Create, AppPermissions.Pages_Administration_HistoryTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetHistoryTypeForEditOutput getHistoryTypeForEditOutput;

            if (id.HasValue)
            {
                getHistoryTypeForEditOutput = await _historyTypesAppService.GetHistoryTypeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getHistoryTypeForEditOutput = new GetHistoryTypeForEditOutput
                {
                    HistoryType = new CreateOrEditHistoryTypeDto()
                };
            }

            var viewModel = new CreateOrEditHistoryTypeModalViewModel()
            {
                HistoryType = getHistoryTypeForEditOutput.HistoryType,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewHistoryTypeModal(int id)
        {
            var getHistoryTypeForViewDto = await _historyTypesAppService.GetHistoryTypeForView(id);

            var model = new HistoryTypeViewModel()
            {
                HistoryType = getHistoryTypeForViewDto.HistoryType
            };

            return PartialView("_ViewHistoryTypeModal", model);
        }

    }
}