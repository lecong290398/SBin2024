using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.StatusDevices.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.StatusDevices
{
    public interface IStatusDevicesAppService : IApplicationService
    {
        Task<PagedResultDto<GetStatusDeviceForViewDto>> GetAll(GetAllStatusDevicesInput input);

        Task<GetStatusDeviceForViewDto> GetStatusDeviceForView(int id);

        Task<GetStatusDeviceForEditOutput> GetStatusDeviceForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditStatusDeviceDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetStatusDevicesToExcel(GetAllStatusDevicesForExcelInput input);

    }
}