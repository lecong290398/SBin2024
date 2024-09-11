using DTKH2024.SbinSolution.StatusDevices;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.Devices.Exporting;
using DTKH2024.SbinSolution.Devices.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.Devices
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Devices)]
    public class DevicesAppService : SbinSolutionAppServiceBase, IDevicesAppService
    {
        private readonly IRepository<Device> _deviceRepository;
        private readonly IDevicesExcelExporter _devicesExcelExporter;
        private readonly IRepository<StatusDevice, int> _lookup_statusDeviceRepository;

        public DevicesAppService(IRepository<Device> deviceRepository, IDevicesExcelExporter devicesExcelExporter, IRepository<StatusDevice, int> lookup_statusDeviceRepository)
        {
            _deviceRepository = deviceRepository;
            _devicesExcelExporter = devicesExcelExporter;
            _lookup_statusDeviceRepository = lookup_statusDeviceRepository;

        }

        public virtual async Task<PagedResultDto<GetDeviceForViewDto>> GetAll(GetAllDevicesInput input)
        {

            var filteredDevices = _deviceRepository.GetAll()
                        .Include(e => e.StatusDeviceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Address.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.SensorPlastisAvailableFilter.HasValue && input.SensorPlastisAvailableFilter > -1, e => (input.SensorPlastisAvailableFilter == 1 && e.SensorPlastisAvailable) || (input.SensorPlastisAvailableFilter == 0 && !e.SensorPlastisAvailable))
                        .WhereIf(input.SensorMetalAvailableFilter.HasValue && input.SensorMetalAvailableFilter > -1, e => (input.SensorMetalAvailableFilter == 1 && e.SensorMetalAvailable) || (input.SensorMetalAvailableFilter == 0 && !e.SensorMetalAvailable))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusDeviceNameFilter), e => e.StatusDeviceFk != null && e.StatusDeviceFk.Name == input.StatusDeviceNameFilter);

            var pagedAndFilteredDevices = filteredDevices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var devices = from o in pagedAndFilteredDevices
                          join o1 in _lookup_statusDeviceRepository.GetAll() on o.StatusDeviceId equals o1.Id into j1
                          from s1 in j1.DefaultIfEmpty()

                          select new
                          {

                              o.Name,
                              o.PlastisPoint,
                              o.SensorPlastisAvailable,
                              o.PercentStatusPlastis,
                              o.MetalPoint,
                              o.SensorMetalAvailable,
                              o.PercentStatusMetal,
                              o.PercentStatusOrther,
                              o.ErrorPoint,
                              o.Address,
                              Id = o.Id,
                              StatusDeviceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                          };

            var totalCount = await filteredDevices.CountAsync();

            var dbList = await devices.ToListAsync();
            var results = new List<GetDeviceForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDeviceForViewDto()
                {
                    Device = new DeviceDto
                    {

                        Name = o.Name,
                        PlastisPoint = o.PlastisPoint,
                        SensorPlastisAvailable = o.SensorPlastisAvailable,
                        PercentStatusPlastis = o.PercentStatusPlastis,
                        MetalPoint = o.MetalPoint,
                        SensorMetalAvailable = o.SensorMetalAvailable,
                        PercentStatusMetal = o.PercentStatusMetal,
                        PercentStatusOrther = o.PercentStatusOrther,
                        ErrorPoint = o.ErrorPoint,
                        Address = o.Address,
                        Id = o.Id,
                    },
                    StatusDeviceName = o.StatusDeviceName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDeviceForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetDeviceForViewDto> GetDeviceForView(int id)
        {
            var device = await _deviceRepository.GetAsync(id);

            var output = new GetDeviceForViewDto { Device = ObjectMapper.Map<DeviceDto>(device) };

            if (output.Device.StatusDeviceId != null)
            {
                var _lookupStatusDevice = await _lookup_statusDeviceRepository.FirstOrDefaultAsync((int)output.Device.StatusDeviceId);
                output.StatusDeviceName = _lookupStatusDevice?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Devices_Edit)]
        public virtual async Task<GetDeviceForEditOutput> GetDeviceForEdit(EntityDto input)
        {
            var device = await _deviceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDeviceForEditOutput { Device = ObjectMapper.Map<CreateOrEditDeviceDto>(device) };

            if (output.Device.StatusDeviceId != null)
            {
                var _lookupStatusDevice = await _lookup_statusDeviceRepository.FirstOrDefaultAsync((int)output.Device.StatusDeviceId);
                output.StatusDeviceName = _lookupStatusDevice?.Name?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditDeviceDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Devices_Create)]
        protected virtual async Task Create(CreateOrEditDeviceDto input)
        {
            var device = ObjectMapper.Map<Device>(input);

            await _deviceRepository.InsertAsync(device);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Devices_Edit)]
        protected virtual async Task Update(CreateOrEditDeviceDto input)
        {
            var device = await _deviceRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, device);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Devices_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _deviceRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetDevicesToExcel(GetAllDevicesForExcelInput input)
        {

            var filteredDevices = _deviceRepository.GetAll()
                        .Include(e => e.StatusDeviceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Address.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.SensorPlastisAvailableFilter.HasValue && input.SensorPlastisAvailableFilter > -1, e => (input.SensorPlastisAvailableFilter == 1 && e.SensorPlastisAvailable) || (input.SensorPlastisAvailableFilter == 0 && !e.SensorPlastisAvailable))
                        .WhereIf(input.SensorMetalAvailableFilter.HasValue && input.SensorMetalAvailableFilter > -1, e => (input.SensorMetalAvailableFilter == 1 && e.SensorMetalAvailable) || (input.SensorMetalAvailableFilter == 0 && !e.SensorMetalAvailable))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StatusDeviceNameFilter), e => e.StatusDeviceFk != null && e.StatusDeviceFk.Name == input.StatusDeviceNameFilter);

            var query = (from o in filteredDevices
                         join o1 in _lookup_statusDeviceRepository.GetAll() on o.StatusDeviceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetDeviceForViewDto()
                         {
                             Device = new DeviceDto
                             {
                                 Name = o.Name,
                                 PlastisPoint = o.PlastisPoint,
                                 SensorPlastisAvailable = o.SensorPlastisAvailable,
                                 PercentStatusPlastis = o.PercentStatusPlastis,
                                 MetalPoint = o.MetalPoint,
                                 SensorMetalAvailable = o.SensorMetalAvailable,
                                 PercentStatusMetal = o.PercentStatusMetal,
                                 PercentStatusOrther = o.PercentStatusOrther,
                                 ErrorPoint = o.ErrorPoint,
                                 Address = o.Address,
                                 Id = o.Id
                             },
                             StatusDeviceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var deviceListDtos = await query.ToListAsync();

            return _devicesExcelExporter.ExportToFile(deviceListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Devices)]
        public async Task<List<DeviceStatusDeviceLookupTableDto>> GetAllStatusDeviceForTableDropdown()
        {
            return await _lookup_statusDeviceRepository.GetAll()
                .Select(statusDevice => new DeviceStatusDeviceLookupTableDto
                {
                    Id = statusDevice.Id,
                    DisplayName = statusDevice == null || statusDevice.Name == null ? "" : statusDevice.Name.ToString()
                }).ToListAsync();
        }

    }
}