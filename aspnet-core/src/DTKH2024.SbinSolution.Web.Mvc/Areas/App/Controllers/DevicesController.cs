using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.Devices;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.Devices;
using DTKH2024.SbinSolution.Devices.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Devices)]
    public class DevicesController : SbinSolutionControllerBase
    {
        private readonly IDevicesAppService _devicesAppService;

        public DevicesController(IDevicesAppService devicesAppService)
        {
            _devicesAppService = devicesAppService;

        }

        public ActionResult Index()
        {
            var model = new DevicesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_Devices_Create, AppPermissions.Pages_Administration_Devices_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetDeviceForEditOutput getDeviceForEditOutput;

            if (id.HasValue)
            {
                getDeviceForEditOutput = await _devicesAppService.GetDeviceForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getDeviceForEditOutput = new GetDeviceForEditOutput
                {
                    Device = new CreateOrEditDeviceDto()
                };
            }

            var viewModel = new CreateOrEditDeviceModalViewModel()
            {
                Device = getDeviceForEditOutput.Device,
                StatusDeviceName = getDeviceForEditOutput.StatusDeviceName,
                DeviceStatusDeviceList = await _devicesAppService.GetAllStatusDeviceForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewDeviceModal(int id)
        {
            var getDeviceForViewDto = await _devicesAppService.GetDeviceForView(id);

            var model = new DeviceViewModel()
            {
                Device = getDeviceForViewDto.Device
                ,
                StatusDeviceName = getDeviceForViewDto.StatusDeviceName

            };

            return PartialView("_ViewDeviceModal", model);
        }

    }
}