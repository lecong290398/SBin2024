using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.StatusDevices;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.StatusDevices;
using DTKH2024.SbinSolution.StatusDevices.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_StatusDevices)]
    public class StatusDevicesController : SbinSolutionControllerBase
    {
        private readonly IStatusDevicesAppService _statusDevicesAppService;

        public StatusDevicesController(IStatusDevicesAppService statusDevicesAppService)
        {
            _statusDevicesAppService = statusDevicesAppService;

        }

        public ActionResult Index()
        {
            var model = new StatusDevicesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_StatusDevices_Create, AppPermissions.Pages_Administration_StatusDevices_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetStatusDeviceForEditOutput getStatusDeviceForEditOutput;

            if (id.HasValue)
            {
                getStatusDeviceForEditOutput = await _statusDevicesAppService.GetStatusDeviceForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getStatusDeviceForEditOutput = new GetStatusDeviceForEditOutput
                {
                    StatusDevice = new CreateOrEditStatusDeviceDto()
                };
            }

            var viewModel = new CreateOrEditStatusDeviceModalViewModel()
            {
                StatusDevice = getStatusDeviceForEditOutput.StatusDevice,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewStatusDeviceModal(int id)
        {
            var getStatusDeviceForViewDto = await _statusDevicesAppService.GetStatusDeviceForView(id);

            var model = new StatusDeviceViewModel()
            {
                StatusDevice = getStatusDeviceForViewDto.StatusDevice
            };

            return PartialView("_ViewStatusDeviceModal", model);
        }

    }
}