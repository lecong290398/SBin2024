using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.StatusDevices.Exporting;
using DTKH2024.SbinSolution.StatusDevices.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.StatusDevices
{
    [AbpAuthorize(AppPermissions.Pages_Administration_StatusDevices)]
    public class StatusDevicesAppService : SbinSolutionAppServiceBase, IStatusDevicesAppService
    {
        private readonly IRepository<StatusDevice> _statusDeviceRepository;
        private readonly IStatusDevicesExcelExporter _statusDevicesExcelExporter;

        public StatusDevicesAppService(IRepository<StatusDevice> statusDeviceRepository, IStatusDevicesExcelExporter statusDevicesExcelExporter)
        {
            _statusDeviceRepository = statusDeviceRepository;
            _statusDevicesExcelExporter = statusDevicesExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetStatusDeviceForViewDto>> GetAll(GetAllStatusDevicesInput input)
        {

            var filteredStatusDevices = _statusDeviceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredStatusDevices = filteredStatusDevices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var statusDevices = from o in pagedAndFilteredStatusDevices
                                select new
                                {

                                    o.Name,
                                    o.Color,
                                    Id = o.Id
                                };

            var totalCount = await filteredStatusDevices.CountAsync();

            var dbList = await statusDevices.ToListAsync();
            var results = new List<GetStatusDeviceForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStatusDeviceForViewDto()
                {
                    StatusDevice = new StatusDeviceDto
                    {

                        Name = o.Name,
                        Color = o.Color,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStatusDeviceForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetStatusDeviceForViewDto> GetStatusDeviceForView(int id)
        {
            var statusDevice = await _statusDeviceRepository.GetAsync(id);

            var output = new GetStatusDeviceForViewDto { StatusDevice = ObjectMapper.Map<StatusDeviceDto>(statusDevice) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_StatusDevices_Edit)]
        public virtual async Task<GetStatusDeviceForEditOutput> GetStatusDeviceForEdit(EntityDto input)
        {
            var statusDevice = await _statusDeviceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStatusDeviceForEditOutput { StatusDevice = ObjectMapper.Map<CreateOrEditStatusDeviceDto>(statusDevice) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditStatusDeviceDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_StatusDevices_Create)]
        protected virtual async Task Create(CreateOrEditStatusDeviceDto input)
        {
            var statusDevice = ObjectMapper.Map<StatusDevice>(input);

            await _statusDeviceRepository.InsertAsync(statusDevice);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_StatusDevices_Edit)]
        protected virtual async Task Update(CreateOrEditStatusDeviceDto input)
        {
            var statusDevice = await _statusDeviceRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, statusDevice);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_StatusDevices_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _statusDeviceRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetStatusDevicesToExcel(GetAllStatusDevicesForExcelInput input)
        {

            var filteredStatusDevices = _statusDeviceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredStatusDevices
                         select new GetStatusDeviceForViewDto()
                         {
                             StatusDevice = new StatusDeviceDto
                             {
                                 Name = o.Name,
                                 Color = o.Color,
                                 Id = o.Id
                             }
                         });

            var statusDeviceListDtos = await query.ToListAsync();

            return _statusDevicesExcelExporter.ExportToFile(statusDeviceListDtos);
        }

    }
}