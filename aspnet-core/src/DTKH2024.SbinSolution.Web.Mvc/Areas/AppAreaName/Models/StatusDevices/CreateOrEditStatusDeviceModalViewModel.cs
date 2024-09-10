using DTKH2024.SbinSolution.StatusDevices.Dtos;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.StatusDevices
{
    public class CreateOrEditStatusDeviceModalViewModel
    {
        public CreateOrEditStatusDeviceDto StatusDevice { get; set; }

        public bool IsEditMode => StatusDevice.Id.HasValue;
    }
}