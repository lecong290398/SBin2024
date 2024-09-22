using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.ProductPromotions;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.WareHouseGifts.Exporting;
using DTKH2024.SbinSolution.WareHouseGifts.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;
using Abp.Runtime.Session;

namespace DTKH2024.SbinSolution.WareHouseGifts
{
    [AbpAuthorize(AppPermissions.Pages_WareHouseGifts)]
    public class WareHouseGiftsAppService : SbinSolutionAppServiceBase, IWareHouseGiftsAppService
    {
        private readonly IRepository<WareHouseGift> _wareHouseGiftRepository;
        private readonly IWareHouseGiftsExcelExporter _wareHouseGiftsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<ProductPromotion, int> _lookup_productPromotionRepository;
        private readonly IAbpSession _abpSession;

        public WareHouseGiftsAppService(IRepository<WareHouseGift> wareHouseGiftRepository
            , IWareHouseGiftsExcelExporter wareHouseGiftsExcelExporter, IRepository<User, long> lookup_userRepository
            , IRepository<ProductPromotion, int> lookup_productPromotionRepository, IAbpSession abpSession)
        {
            _wareHouseGiftRepository = wareHouseGiftRepository;
            _wareHouseGiftsExcelExporter = wareHouseGiftsExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_productPromotionRepository = lookup_productPromotionRepository;
            _abpSession = abpSession;
        }

        public virtual async Task<PagedResultDto<GetWareHouseGiftForViewDto>> GetAll(GetAllWareHouseGiftsInput input)
        {

            var filteredWareHouseGifts = _wareHouseGiftRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.ProductPromotionFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsUsedFilter.HasValue && input.IsUsedFilter > -1, e => (input.IsUsedFilter == 1 && e.IsUsed) || (input.IsUsedFilter == 0 && !e.IsUsed))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductPromotionPromotionCodeFilter), e => e.ProductPromotionFk != null && e.ProductPromotionFk.PromotionCode == input.ProductPromotionPromotionCodeFilter);

            var pagedAndFilteredWareHouseGifts = filteredWareHouseGifts
                .OrderBy(input.Sorting ?? "id asc")
            .PageBy(input);

            var userID = _abpSession.GetUserId();
            if (userID != AppConsts.UserIdAdmin)
            {
                filteredWareHouseGifts = filteredWareHouseGifts.Where(e => e.UserFk != null && e.UserFk.Id == userID);
            }


            var wareHouseGifts = from o in pagedAndFilteredWareHouseGifts
                                 join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_productPromotionRepository.GetAll() on o.ProductPromotionId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 select new
                                 {

                                     o.Code,
                                     o.IsUsed,
                                     Id = o.Id,
                                     UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     ProductPromotionPromotionCode = s2 == null || s2.PromotionCode == null ? "" : s2.PromotionCode.ToString()
                                 };

            var totalCount = await filteredWareHouseGifts.CountAsync();

            var dbList = await wareHouseGifts.ToListAsync();
            var results = new List<GetWareHouseGiftForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetWareHouseGiftForViewDto()
                {
                    WareHouseGift = new WareHouseGiftDto
                    {

                        Code = o.Code,
                        IsUsed = o.IsUsed,
                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    ProductPromotionPromotionCode = o.ProductPromotionPromotionCode
                };

                results.Add(res);
            }

            return new PagedResultDto<GetWareHouseGiftForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetWareHouseGiftForViewDto> GetWareHouseGiftForView(int id)
        {
            var wareHouseGift = await _wareHouseGiftRepository.GetAsync(id);

            var output = new GetWareHouseGiftForViewDto { WareHouseGift = ObjectMapper.Map<WareHouseGiftDto>(wareHouseGift) };

            if (output.WareHouseGift.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.WareHouseGift.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.WareHouseGift.ProductPromotionId != null)
            {
                var _lookupProductPromotion = await _lookup_productPromotionRepository.FirstOrDefaultAsync((int)output.WareHouseGift.ProductPromotionId);
                output.ProductPromotionPromotionCode = _lookupProductPromotion?.PromotionCode?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_WareHouseGifts_Edit)]
        public virtual async Task<GetWareHouseGiftForEditOutput> GetWareHouseGiftForEdit(EntityDto input)
        {
            var wareHouseGift = await _wareHouseGiftRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetWareHouseGiftForEditOutput { WareHouseGift = ObjectMapper.Map<CreateOrEditWareHouseGiftDto>(wareHouseGift) };

            if (output.WareHouseGift.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.WareHouseGift.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.WareHouseGift.ProductPromotionId != null)
            {
                var _lookupProductPromotion = await _lookup_productPromotionRepository.FirstOrDefaultAsync((int)output.WareHouseGift.ProductPromotionId);
                output.ProductPromotionPromotionCode = _lookupProductPromotion?.PromotionCode?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditWareHouseGiftDto input)
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

        [AbpAuthorize(AppPermissions.Pages_WareHouseGifts_Create)]
        protected virtual async Task Create(CreateOrEditWareHouseGiftDto input)
        {
            var wareHouseGift = ObjectMapper.Map<WareHouseGift>(input);
            wareHouseGift.Code = AppConsts.getCodeRandom(AppConsts.keyPerfixWareHouseGift);
            await _wareHouseGiftRepository.InsertAsync(wareHouseGift);

        }

        [AbpAuthorize(AppPermissions.Pages_WareHouseGifts_Edit)]
        protected virtual async Task Update(CreateOrEditWareHouseGiftDto input)
        {
            var wareHouseGift = await _wareHouseGiftRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, wareHouseGift);

        }

        [AbpAuthorize(AppPermissions.Pages_WareHouseGifts_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _wareHouseGiftRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetWareHouseGiftsToExcel(GetAllWareHouseGiftsForExcelInput input)
        {

            var filteredWareHouseGifts = _wareHouseGiftRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.ProductPromotionFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.IsUsedFilter.HasValue && input.IsUsedFilter > -1, e => (input.IsUsedFilter == 1 && e.IsUsed) || (input.IsUsedFilter == 0 && !e.IsUsed))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductPromotionPromotionCodeFilter), e => e.ProductPromotionFk != null && e.ProductPromotionFk.PromotionCode == input.ProductPromotionPromotionCodeFilter);

            var query = (from o in filteredWareHouseGifts
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productPromotionRepository.GetAll() on o.ProductPromotionId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetWareHouseGiftForViewDto()
                         {
                             WareHouseGift = new WareHouseGiftDto
                             {
                                 Code = o.Code,
                                 IsUsed = o.IsUsed,
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductPromotionPromotionCode = s2 == null || s2.PromotionCode == null ? "" : s2.PromotionCode.ToString()
                         });

            var wareHouseGiftListDtos = await query.ToListAsync();

            return _wareHouseGiftsExcelExporter.ExportToFile(wareHouseGiftListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_WareHouseGifts)]
        public async Task<PagedResultDto<WareHouseGiftUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WareHouseGiftUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new WareHouseGiftUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<WareHouseGiftUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_WareHouseGifts)]
        public async Task<PagedResultDto<WareHouseGiftProductPromotionLookupTableDto>> GetAllProductPromotionForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productPromotionRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.PromotionCode != null && e.PromotionCode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productPromotionList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<WareHouseGiftProductPromotionLookupTableDto>();
            foreach (var productPromotion in productPromotionList)
            {
                lookupTableDtoList.Add(new WareHouseGiftProductPromotionLookupTableDto
                {
                    Id = productPromotion.Id,
                    DisplayName = productPromotion.PromotionCode?.ToString()
                });
            }

            return new PagedResultDto<WareHouseGiftProductPromotionLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}