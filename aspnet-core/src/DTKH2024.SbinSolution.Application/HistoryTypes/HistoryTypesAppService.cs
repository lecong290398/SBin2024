using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.HistoryTypes.Exporting;
using DTKH2024.SbinSolution.HistoryTypes.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.HistoryTypes
{
    [AbpAuthorize(AppPermissions.Pages_Administration_HistoryTypes)]
    public class HistoryTypesAppService : SbinSolutionAppServiceBase, IHistoryTypesAppService
    {
        private readonly IRepository<HistoryType> _historyTypeRepository;
        private readonly IHistoryTypesExcelExporter _historyTypesExcelExporter;

        public HistoryTypesAppService(IRepository<HistoryType> historyTypeRepository, IHistoryTypesExcelExporter historyTypesExcelExporter)
        {
            _historyTypeRepository = historyTypeRepository;
            _historyTypesExcelExporter = historyTypesExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetHistoryTypeForViewDto>> GetAll(GetAllHistoryTypesInput input)
        {

            var filteredHistoryTypes = _historyTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredHistoryTypes = filteredHistoryTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var historyTypes = from o in pagedAndFilteredHistoryTypes
                               select new
                               {

                                   o.Name,
                                   o.Description,
                                   o.Color,
                                   Id = o.Id
                               };

            var totalCount = await filteredHistoryTypes.CountAsync();

            var dbList = await historyTypes.ToListAsync();
            var results = new List<GetHistoryTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHistoryTypeForViewDto()
                {
                    HistoryType = new HistoryTypeDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Color = o.Color,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHistoryTypeForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetHistoryTypeForViewDto> GetHistoryTypeForView(int id)
        {
            var historyType = await _historyTypeRepository.GetAsync(id);

            var output = new GetHistoryTypeForViewDto { HistoryType = ObjectMapper.Map<HistoryTypeDto>(historyType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_HistoryTypes_Edit)]
        public virtual async Task<GetHistoryTypeForEditOutput> GetHistoryTypeForEdit(EntityDto input)
        {
            var historyType = await _historyTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHistoryTypeForEditOutput { HistoryType = ObjectMapper.Map<CreateOrEditHistoryTypeDto>(historyType) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditHistoryTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_HistoryTypes_Create)]
        protected virtual async Task Create(CreateOrEditHistoryTypeDto input)
        {
            var historyType = ObjectMapper.Map<HistoryType>(input);

            await _historyTypeRepository.InsertAsync(historyType);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_HistoryTypes_Edit)]
        protected virtual async Task Update(CreateOrEditHistoryTypeDto input)
        {
            var historyType = await _historyTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, historyType);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_HistoryTypes_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _historyTypeRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetHistoryTypesToExcel(GetAllHistoryTypesForExcelInput input)
        {

            var filteredHistoryTypes = _historyTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredHistoryTypes
                         select new GetHistoryTypeForViewDto()
                         {
                             HistoryType = new HistoryTypeDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Color = o.Color,
                                 Id = o.Id
                             }
                         });

            var historyTypeListDtos = await query.ToListAsync();

            return _historyTypesExcelExporter.ExportToFile(historyTypeListDtos);
        }

    }
}