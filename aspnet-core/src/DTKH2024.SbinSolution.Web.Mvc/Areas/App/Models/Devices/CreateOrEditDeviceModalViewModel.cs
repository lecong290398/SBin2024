using DTKH2024.SbinSolution.Devices.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Devices
{
    public class CreateOrEditDeviceModalViewModel
    {
        public CreateOrEditDeviceDto Device { get; set; }

        public string StatusDeviceName { get; set; }

        public List<DeviceStatusDeviceLookupTableDto> DeviceStatusDeviceList { get; set; }

        public bool IsEditMode => Device.Id.HasValue;
    }
}