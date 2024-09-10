using DTKH2024.SbinSolution.RankLevels;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.BenefitsRankLevels.Exporting;
using DTKH2024.SbinSolution.BenefitsRankLevels.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.BenefitsRankLevels
{
    [AbpAuthorize(AppPermissions.Pages_Administration_BenefitsRankLevels)]
    public class BenefitsRankLevelsAppService : SbinSolutionAppServiceBase, IBenefitsRankLevelsAppService
    {
        private readonly IRepository<BenefitsRankLevel> _benefitsRankLevelRepository;
        private readonly IBenefitsRankLevelsExcelExporter _benefitsRankLevelsExcelExporter;
        private readonly IRepository<RankLevel, int> _lookup_rankLevelRepository;

        public BenefitsRankLevelsAppService(IRepository<BenefitsRankLevel> benefitsRankLevelRepository, IBenefitsRankLevelsExcelExporter benefitsRankLevelsExcelExporter, IRepository<RankLevel, int> lookup_rankLevelRepository)
        {
            _benefitsRankLevelRepository = benefitsRankLevelRepository;
            _benefitsRankLevelsExcelExporter = benefitsRankLevelsExcelExporter;
            _lookup_rankLevelRepository = lookup_rankLevelRepository;

        }

        public virtual async Task<PagedResultDto<GetBenefitsRankLevelForViewDto>> GetAll(GetAllBenefitsRankLevelsInput input)
        {

            var filteredBenefitsRankLevels = _benefitsRankLevelRepository.GetAll()
                        .Include(e => e.RankLevelFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RankLevelNameFilter), e => e.RankLevelFk != null && e.RankLevelFk.Name == input.RankLevelNameFilter);

            var pagedAndFilteredBenefitsRankLevels = filteredBenefitsRankLevels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var benefitsRankLevels = from o in pagedAndFilteredBenefitsRankLevels
                                     join o1 in _lookup_rankLevelRepository.GetAll() on o.RankLevelId equals o1.Id into j1
                                     from s1 in j1.DefaultIfEmpty()

                                     select new
                                     {

                                         o.Name,
                                         o.Description,
                                         Id = o.Id,
                                         RankLevelName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                     };

            var totalCount = await filteredBenefitsRankLevels.CountAsync();

            var dbList = await benefitsRankLevels.ToListAsync();
            var results = new List<GetBenefitsRankLevelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBenefitsRankLevelForViewDto()
                {
                    BenefitsRankLevel = new BenefitsRankLevelDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                    },
                    RankLevelName = o.RankLevelName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBenefitsRankLevelForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetBenefitsRankLevelForViewDto> GetBenefitsRankLevelForView(int id)
        {
            var benefitsRankLevel = await _benefitsRankLevelRepository.GetAsync(id);

            var output = new GetBenefitsRankLevelForViewDto { BenefitsRankLevel = ObjectMapper.Map<BenefitsRankLevelDto>(benefitsRankLevel) };

            if (output.BenefitsRankLevel.RankLevelId != null)
            {
                var _lookupRankLevel = await _lookup_rankLevelRepository.FirstOrDefaultAsync((int)output.BenefitsRankLevel.RankLevelId);
                output.RankLevelName = _lookupRankLevel?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_BenefitsRankLevels_Edit)]
        public virtual async Task<GetBenefitsRankLevelForEditOutput> GetBenefitsRankLevelForEdit(EntityDto input)
        {
            var benefitsRankLevel = await _benefitsRankLevelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBenefitsRankLevelForEditOutput { BenefitsRankLevel = ObjectMapper.Map<CreateOrEditBenefitsRankLevelDto>(benefitsRankLevel) };

            if (output.BenefitsRankLevel.RankLevelId != null)
            {
                var _lookupRankLevel = await _lookup_rankLevelRepository.FirstOrDefaultAsync((int)output.BenefitsRankLevel.RankLevelId);
                output.RankLevelName = _lookupRankLevel?.Name?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditBenefitsRankLevelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_BenefitsRankLevels_Create)]
        protected virtual async Task Create(CreateOrEditBenefitsRankLevelDto input)
        {
            var benefitsRankLevel = ObjectMapper.Map<BenefitsRankLevel>(input);

            await _benefitsRankLevelRepository.InsertAsync(benefitsRankLevel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_BenefitsRankLevels_Edit)]
        protected virtual async Task Update(CreateOrEditBenefitsRankLevelDto input)
        {
            var benefitsRankLevel = await _benefitsRankLevelRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, benefitsRankLevel);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_BenefitsRankLevels_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _benefitsRankLevelRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetBenefitsRankLevelsToExcel(GetAllBenefitsRankLevelsForExcelInput input)
        {

            var filteredBenefitsRankLevels = _benefitsRankLevelRepository.GetAll()
                        .Include(e => e.RankLevelFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RankLevelNameFilter), e => e.RankLevelFk != null && e.RankLevelFk.Name == input.RankLevelNameFilter);

            var query = (from o in filteredBenefitsRankLevels
                         join o1 in _lookup_rankLevelRepository.GetAll() on o.RankLevelId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetBenefitsRankLevelForViewDto()
                         {
                             BenefitsRankLevel = new BenefitsRankLevelDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             RankLevelName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var benefitsRankLevelListDtos = await query.ToListAsync();

            return _benefitsRankLevelsExcelExporter.ExportToFile(benefitsRankLevelListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_BenefitsRankLevels)]
        public async Task<List<BenefitsRankLevelRankLevelLookupTableDto>> GetAllRankLevelForTableDropdown()
        {
            return await _lookup_rankLevelRepository.GetAll()
                .Select(rankLevel => new BenefitsRankLevelRankLevelLookupTableDto
                {
                    Id = rankLevel.Id,
                    DisplayName = rankLevel == null || rankLevel.Name == null ? "" : rankLevel.Name.ToString()
                }).ToListAsync();
        }

    }
}