using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Devices.Dtos;
using DTKH2024.SbinSolution.Dto;
using System.Collections.Generic;

namespace DTKH2024.SbinSolution.Devices
{
    public interface IDevicesAppService : IApplicationService
    {
        Task<PagedResultDto<GetDeviceForViewDto>> GetAll(GetAllDevicesInput input);

        Task<GetDeviceForViewDto> GetDeviceForView(int id);

        Task<GetDeviceForEditOutput> GetDeviceForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDeviceDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetDevicesToExcel(GetAllDevicesForExcelInput input);

        Task<List<DeviceStatusDeviceLookupTableDto>> GetAllStatusDeviceForTableDropdown();

        Task<PagedResultDto<DeviceUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);


        Task<GetDeviceForViewDto> GetDeviceForViewDemo(int id);

    }
}