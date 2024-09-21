using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.TransactionBins;
using DTKH2024.SbinSolution.WareHouseGifts;
using DTKH2024.SbinSolution.HistoryTypes;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.OrderHistories.Exporting;
using DTKH2024.SbinSolution.OrderHistories.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;
using Abp.Runtime.Session;

namespace DTKH2024.SbinSolution.OrderHistories
{
    [AbpAuthorize(AppPermissions.Pages_OrderHistories)]
    public class OrderHistoriesAppService : SbinSolutionAppServiceBase, IOrderHistoriesAppService
    {
        private readonly IRepository<OrderHistory> _orderHistoryRepository;
        private readonly IOrderHistoriesExcelExporter _orderHistoriesExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<TransactionBin, int> _lookup_transactionBinRepository;
        private readonly IRepository<WareHouseGift, int> _lookup_wareHouseGiftRepository;
        private readonly IRepository<HistoryType, int> _lookup_historyTypeRepository;
        private readonly IAbpSession _abpSession;

        public OrderHistoriesAppService(IRepository<OrderHistory> orderHistoryRepository, IAbpSession abpSession, IOrderHistoriesExcelExporter orderHistoriesExcelExporter, IRepository<User, long> lookup_userRepository, IRepository<TransactionBin, int> lookup_transactionBinRepository, IRepository<WareHouseGift, int> lookup_wareHouseGiftRepository, IRepository<HistoryType, int> lookup_historyTypeRepository)
        {
            _orderHistoryRepository = orderHistoryRepository;
            _orderHistoriesExcelExporter = orderHistoriesExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_transactionBinRepository = lookup_transactionBinRepository;
            _lookup_wareHouseGiftRepository = lookup_wareHouseGiftRepository;
            _lookup_historyTypeRepository = lookup_historyTypeRepository;
            _abpSession = abpSession;
        }

        public virtual async Task<PagedResultDto<GetOrderHistoryForViewDto>> GetAll(GetAllOrderHistoriesInput input)
        {

            var filteredOrderHistories = _orderHistoryRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.TransactionBinFk)
                        .Include(e => e.WareHouseGiftFk)
                        .Include(e => e.HistoryTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter) || e.Reason.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionBinTransactionCodeFilter), e => e.TransactionBinFk != null && e.TransactionBinFk.TransactionCode == input.TransactionBinTransactionCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WareHouseGiftCodeFilter), e => e.WareHouseGiftFk != null && e.WareHouseGiftFk.Code == input.WareHouseGiftCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HistoryTypeNameFilter), e => e.HistoryTypeFk != null && e.HistoryTypeFk.Name == input.HistoryTypeNameFilter);
            var userID = _abpSession.GetUserId();
            if (userID != AppConsts.UserIdAdmin)
            {
                filteredOrderHistories.Where(e => e.UserFk != null && e.UserFk.Id == userID);
            }
            var pagedAndFilteredOrderHistories = filteredOrderHistories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderHistories = from o in pagedAndFilteredOrderHistories
                                 join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_transactionBinRepository.GetAll() on o.TransactionBinId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 join o3 in _lookup_wareHouseGiftRepository.GetAll() on o.WareHouseGiftId equals o3.Id into j3
                                 from s3 in j3.DefaultIfEmpty()

                                 join o4 in _lookup_historyTypeRepository.GetAll() on o.HistoryTypeId equals o4.Id into j4
                                 from s4 in j4.DefaultIfEmpty()

                                 select new
                                 {

                                     o.Description,
                                     o.Reason,
                                     o.Point,
                                     Id = o.Id,
                                     UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                     TransactionBinTransactionCode = s2 == null || s2.TransactionCode == null ? "" : s2.TransactionCode.ToString(),
                                     WareHouseGiftCode = s3 == null || s3.Code == null ? "" : s3.Code.ToString(),
                                     HistoryTypeName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                                 };

            var totalCount = await filteredOrderHistories.CountAsync();

            var dbList = await orderHistories.ToListAsync();
            var results = new List<GetOrderHistoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderHistoryForViewDto()
                {
                    OrderHistory = new OrderHistoryDto
                    {

                        Description = o.Description,
                        Reason = o.Reason,
                        Point = o.Point,
                        Id = o.Id,
                    },
                    UserName = o.UserName,
                    TransactionBinTransactionCode = o.TransactionBinTransactionCode,
                    WareHouseGiftCode = o.WareHouseGiftCode,
                    HistoryTypeName = o.HistoryTypeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderHistoryForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetOrderHistoryForViewDto> GetOrderHistoryForView(int id)
        {
            var orderHistory = await _orderHistoryRepository.GetAsync(id);

            var output = new GetOrderHistoryForViewDto { OrderHistory = ObjectMapper.Map<OrderHistoryDto>(orderHistory) };

            if (output.OrderHistory.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.OrderHistory.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.OrderHistory.TransactionBinId != null)
            {
                var _lookupTransactionBin = await _lookup_transactionBinRepository.FirstOrDefaultAsync((int)output.OrderHistory.TransactionBinId);
                output.TransactionBinTransactionCode = _lookupTransactionBin?.TransactionCode?.ToString();
            }

            if (output.OrderHistory.WareHouseGiftId != null)
            {
                var _lookupWareHouseGift = await _lookup_wareHouseGiftRepository.FirstOrDefaultAsync((int)output.OrderHistory.WareHouseGiftId);
                output.WareHouseGiftCode = _lookupWareHouseGift?.Code?.ToString();
            }

            if (output.OrderHistory.HistoryTypeId != null)
            {
                var _lookupHistoryType = await _lookup_historyTypeRepository.FirstOrDefaultAsync((int)output.OrderHistory.HistoryTypeId);
                output.HistoryTypeName = _lookupHistoryType?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderHistories_Edit)]
        public virtual async Task<GetOrderHistoryForEditOutput> GetOrderHistoryForEdit(EntityDto input)
        {
            var orderHistory = await _orderHistoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderHistoryForEditOutput { OrderHistory = ObjectMapper.Map<CreateOrEditOrderHistoryDto>(orderHistory) };

            if (output.OrderHistory.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.OrderHistory.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.OrderHistory.TransactionBinId != null)
            {
                var _lookupTransactionBin = await _lookup_transactionBinRepository.FirstOrDefaultAsync((int)output.OrderHistory.TransactionBinId);
                output.TransactionBinTransactionCode = _lookupTransactionBin?.TransactionCode?.ToString();
            }

            if (output.OrderHistory.WareHouseGiftId != null)
            {
                var _lookupWareHouseGift = await _lookup_wareHouseGiftRepository.FirstOrDefaultAsync((int)output.OrderHistory.WareHouseGiftId);
                output.WareHouseGiftCode = _lookupWareHouseGift?.Code?.ToString();
            }

            if (output.OrderHistory.HistoryTypeId != null)
            {
                var _lookupHistoryType = await _lookup_historyTypeRepository.FirstOrDefaultAsync((int)output.OrderHistory.HistoryTypeId);
                output.HistoryTypeName = _lookupHistoryType?.Name?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditOrderHistoryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderHistories_Create)]
        protected virtual async Task Create(CreateOrEditOrderHistoryDto input)
        {
            var orderHistory = ObjectMapper.Map<OrderHistory>(input);

            await _orderHistoryRepository.InsertAsync(orderHistory);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderHistories_Edit)]
        protected virtual async Task Update(CreateOrEditOrderHistoryDto input)
        {
            var orderHistory = await _orderHistoryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, orderHistory);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderHistories_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _orderHistoryRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetOrderHistoriesToExcel(GetAllOrderHistoriesForExcelInput input)
        {

            var filteredOrderHistories = _orderHistoryRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.TransactionBinFk)
                        .Include(e => e.WareHouseGiftFk)
                        .Include(e => e.HistoryTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter) || e.Reason.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionBinTransactionCodeFilter), e => e.TransactionBinFk != null && e.TransactionBinFk.TransactionCode == input.TransactionBinTransactionCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WareHouseGiftCodeFilter), e => e.WareHouseGiftFk != null && e.WareHouseGiftFk.Code == input.WareHouseGiftCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HistoryTypeNameFilter), e => e.HistoryTypeFk != null && e.HistoryTypeFk.Name == input.HistoryTypeNameFilter);

            var query = (from o in filteredOrderHistories
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_transactionBinRepository.GetAll() on o.TransactionBinId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_wareHouseGiftRepository.GetAll() on o.WareHouseGiftId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_historyTypeRepository.GetAll() on o.HistoryTypeId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetOrderHistoryForViewDto()
                         {
                             OrderHistory = new OrderHistoryDto
                             {
                                 Description = o.Description,
                                 Reason = o.Reason,
                                 Point = o.Point,
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             TransactionBinTransactionCode = s2 == null || s2.TransactionCode == null ? "" : s2.TransactionCode.ToString(),
                             WareHouseGiftCode = s3 == null || s3.Code == null ? "" : s3.Code.ToString(),
                             HistoryTypeName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var orderHistoryListDtos = await query.ToListAsync();

            return _orderHistoriesExcelExporter.ExportToFile(orderHistoryListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderHistories)]
        public async Task<PagedResultDto<OrderHistoryUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderHistoryUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new OrderHistoryUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderHistoryUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderHistories)]
        public async Task<PagedResultDto<OrderHistoryTransactionBinLookupTableDto>> GetAllTransactionBinForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_transactionBinRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.TransactionCode != null && e.TransactionCode.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var transactionBinList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderHistoryTransactionBinLookupTableDto>();
            foreach (var transactionBin in transactionBinList)
            {
                lookupTableDtoList.Add(new OrderHistoryTransactionBinLookupTableDto
                {
                    Id = transactionBin.Id,
                    DisplayName = transactionBin.TransactionCode?.ToString()
                });
            }

            return new PagedResultDto<OrderHistoryTransactionBinLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_OrderHistories)]
        public async Task<PagedResultDto<OrderHistoryWareHouseGiftLookupTableDto>> GetAllWareHouseGiftForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_wareHouseGiftRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Code != null && e.Code.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var wareHouseGiftList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderHistoryWareHouseGiftLookupTableDto>();
            foreach (var wareHouseGift in wareHouseGiftList)
            {
                lookupTableDtoList.Add(new OrderHistoryWareHouseGiftLookupTableDto
                {
                    Id = wareHouseGift.Id,
                    DisplayName = wareHouseGift.Code?.ToString()
                });
            }

            return new PagedResultDto<OrderHistoryWareHouseGiftLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_OrderHistories)]
        public async Task<List<OrderHistoryHistoryTypeLookupTableDto>> GetAllHistoryTypeForTableDropdown()
        {
            return await _lookup_historyTypeRepository.GetAll()
                .Select(historyType => new OrderHistoryHistoryTypeLookupTableDto
                {
                    Id = historyType.Id,
                    DisplayName = historyType == null || historyType.Name == null ? "" : historyType.Name.ToString()
                }).ToListAsync();
        }

    }
}