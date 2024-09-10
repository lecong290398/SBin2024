using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.RankLevels.Exporting;
using DTKH2024.SbinSolution.RankLevels.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.RankLevels
{
    [AbpAuthorize(AppPermissions.Pages_Administration_RankLevels)]
    public class RankLevelsAppService : SbinSolutionAppServiceBase, IRankLevelsAppService
    {
        private readonly IRepository<RankLevel> _rankLevelRepository;
        private readonly IRankLevelsExcelExporter _rankLevelsExcelExporter;

        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public RankLevelsAppService(IRepository<RankLevel> rankLevelRepository, IRankLevelsExcelExporter rankLevelsExcelExporter, ITempFileCacheManager tempFileCacheManager, IBinaryObjectManager binaryObjectManager)
        {
            _rankLevelRepository = rankLevelRepository;
            _rankLevelsExcelExporter = rankLevelsExcelExporter;

            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;

        }

        public virtual async Task<PagedResultDto<GetRankLevelForViewDto>> GetAll(GetAllRankLevelsInput input)
        {

            var filteredRankLevels = _rankLevelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredRankLevels = filteredRankLevels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var rankLevels = from o in pagedAndFilteredRankLevels
                             select new
                             {

                                 o.Name,
                                 o.Description,
                                 o.MinimumPositiveScore,
                                 o.Color,
                                 o.Logo,
                                 Id = o.Id
                             };

            var totalCount = await filteredRankLevels.CountAsync();

            var dbList = await rankLevels.ToListAsync();
            var results = new List<GetRankLevelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetRankLevelForViewDto()
                {
                    RankLevel = new RankLevelDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        MinimumPositiveScore = o.MinimumPositiveScore,
                        Color = o.Color,
                        Logo = o.Logo,
                        Id = o.Id,
                    }
                };
                res.RankLevel.LogoFileName = await GetBinaryFileName(o.Logo);

                results.Add(res);
            }

            return new PagedResultDto<GetRankLevelForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetRankLevelForViewDto> GetRankLevelForView(int id)
        {
            var rankLevel = await _rankLevelRepository.GetAsync(id);

            var output = new GetRankLevelForViewDto { RankLevel = ObjectMapper.Map<RankLevelDto>(rankLevel) };

            output.RankLevel.LogoFileName = await GetBinaryFileName(rankLevel.Logo);

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RankLevels_Edit)]
        public virtual async Task<GetRankLevelForEditOutput> GetRankLevelForEdit(EntityDto input)
        {
            var rankLevel = await _rankLevelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRankLevelForEditOutput { RankLevel = ObjectMapper.Map<CreateOrEditRankLevelDto>(rankLevel) };

            output.LogoFileName = await GetBinaryFileName(rankLevel.Logo);

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditRankLevelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_RankLevels_Create)]
        protected virtual async Task Create(CreateOrEditRankLevelDto input)
        {
            var rankLevel = ObjectMapper.Map<RankLevel>(input);

            await _rankLevelRepository.InsertAsync(rankLevel);
            rankLevel.Logo = await GetBinaryObjectFromCache(input.LogoToken);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RankLevels_Edit)]
        protected virtual async Task Update(CreateOrEditRankLevelDto input)
        {
            var rankLevel = await _rankLevelRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, rankLevel);
            rankLevel.Logo = await GetBinaryObjectFromCache(input.LogoToken);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RankLevels_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _rankLevelRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetRankLevelsToExcel(GetAllRankLevelsForExcelInput input)
        {

            var filteredRankLevels = _rankLevelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredRankLevels
                         select new GetRankLevelForViewDto()
                         {
                             RankLevel = new RankLevelDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 MinimumPositiveScore = o.MinimumPositiveScore,
                                 Color = o.Color,
                                 Logo = o.Logo,
                                 Id = o.Id
                             }
                         });

            var rankLevelListDtos = await query.ToListAsync();

            return _rankLevelsExcelExporter.ExportToFile(rankLevelListDtos);
        }

        protected virtual async Task<Guid?> GetBinaryObjectFromCache(string fileToken)
        {
            if (fileToken.IsNullOrWhiteSpace())
            {
                return null;
            }

            var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

            if (fileCache == null)
            {
                throw new UserFriendlyException("There is no such file with the token: " + fileToken);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, fileCache.FileName);
            await _binaryObjectManager.SaveAsync(storedFile);

            return storedFile.Id;
        }

        protected virtual async Task<string> GetBinaryFileName(Guid? fileId)
        {
            if (!fileId.HasValue)
            {
                return null;
            }

            var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value);
            return file?.Description;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_RankLevels_Edit)]
        public virtual async Task RemoveLogoFile(EntityDto input)
        {
            var rankLevel = await _rankLevelRepository.FirstOrDefaultAsync(input.Id);
            if (rankLevel == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!rankLevel.Logo.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _binaryObjectManager.DeleteAsync(rankLevel.Logo.Value);
            rankLevel.Logo = null;
        }

    }
}