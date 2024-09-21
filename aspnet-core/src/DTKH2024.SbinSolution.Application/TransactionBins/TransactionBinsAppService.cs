using DTKH2024.SbinSolution.Devices;
using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.TransactionStatuses;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.TransactionBins.Exporting;
using DTKH2024.SbinSolution.TransactionBins.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;
using Abp.Runtime.Session;

namespace DTKH2024.SbinSolution.TransactionBins
{
    [AbpAuthorize(AppPermissions.Pages_Administration_TransactionBins)]
    public class TransactionBinsAppService : SbinSolutionAppServiceBase, ITransactionBinsAppService
    {
        private readonly IRepository<TransactionBin> _transactionBinRepository;
        private readonly ITransactionBinsExcelExporter _transactionBinsExcelExporter;
        private readonly IRepository<Device, int> _lookup_deviceRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<TransactionStatus, int> _lookup_transactionStatusRepository;
        private readonly IAbpSession _abpSession;

        public TransactionBinsAppService(IRepository<TransactionBin> transactionBinRepository, IAbpSession abpSession, ITransactionBinsExcelExporter transactionBinsExcelExporter, IRepository<Device, int> lookup_deviceRepository, IRepository<User, long> lookup_userRepository, IRepository<TransactionStatus, int> lookup_transactionStatusRepository)
        {
            _transactionBinRepository = transactionBinRepository;
            _transactionBinsExcelExporter = transactionBinsExcelExporter;
            _lookup_deviceRepository = lookup_deviceRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_transactionStatusRepository = lookup_transactionStatusRepository;
            _abpSession = abpSession ?? NullAbpSession.Instance;

        }

        public virtual async Task<PagedResultDto<GetTransactionBinForViewDto>> GetAll(GetAllTransactionBinsInput input)
        {

            var filteredTransactionBins = _transactionBinRepository.GetAll()
                        .Include(e => e.DeviceFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.TransactionStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TransactionCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionCodeFilter), e => e.TransactionCode.Contains(input.TransactionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceNameFilter), e => e.DeviceFk != null && e.DeviceFk.Name == input.DeviceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionStatusNameFilter), e => e.TransactionStatusFk != null && e.TransactionStatusFk.Name == input.TransactionStatusNameFilter);

            var userID = _abpSession.GetUserId();
            if (userID != AppConsts.UserIdAdmin)
            {
                filteredTransactionBins = filteredTransactionBins.Where(e => e.DeviceFk != null && e.DeviceFk.UserId != null && e.DeviceFk.UserId == userID);
            }

            var pagedAndFilteredTransactionBins = filteredTransactionBins
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var transactionBins = from o in pagedAndFilteredTransactionBins
                                  join o1 in _lookup_deviceRepository.GetAll() on o.DeviceId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                                  from s2 in j2.DefaultIfEmpty()

                                  join o3 in _lookup_transactionStatusRepository.GetAll() on o.TransactionStatusId equals o3.Id into j3
                                  from s3 in j3.DefaultIfEmpty()

                                  select new
                                  {

                                      o.PlastisQuantity,
                                      o.PlastisPoint,
                                      o.MetalQuantity,
                                      o.MetalPoint,
                                      o.OrtherQuantity,
                                      o.ErrorPoint,
                                      o.TransactionCode,
                                      Id = o.Id,
                                      DeviceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                      UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                      TransactionStatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                                  };

            var totalCount = await filteredTransactionBins.CountAsync();

            var dbList = await transactionBins.ToListAsync();
            var results = new List<GetTransactionBinForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTransactionBinForViewDto()
                {
                    TransactionBin = new TransactionBinDto
                    {

                        PlastisQuantity = o.PlastisQuantity,
                        PlastisPoint = o.PlastisPoint,
                        MetalQuantity = o.MetalQuantity,
                        MetalPoint = o.MetalPoint,
                        OrtherQuantity = o.OrtherQuantity,
                        ErrorPoint = o.ErrorPoint,
                        TransactionCode = o.TransactionCode,
                        Id = o.Id,
                    },
                    DeviceName = o.DeviceName,
                    UserName = o.UserName,
                    TransactionStatusName = o.TransactionStatusName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTransactionBinForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetTransactionBinForViewDto> GetTransactionBinForView(int id)
        {
            var transactionBin = await _transactionBinRepository.GetAsync(id);

            var output = new GetTransactionBinForViewDto { TransactionBin = ObjectMapper.Map<TransactionBinDto>(transactionBin) };

            if (output.TransactionBin.DeviceId != null)
            {
                var _lookupDevice = await _lookup_deviceRepository.FirstOrDefaultAsync((int)output.TransactionBin.DeviceId);
                output.DeviceName = _lookupDevice?.Name?.ToString();
            }

            if (output.TransactionBin.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TransactionBin.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.TransactionBin.TransactionStatusId != null)
            {
                var _lookupTransactionStatus = await _lookup_transactionStatusRepository.FirstOrDefaultAsync((int)output.TransactionBin.TransactionStatusId);
                output.TransactionStatusName = _lookupTransactionStatus?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionBins_Edit)]
        public virtual async Task<GetTransactionBinForEditOutput> GetTransactionBinForEdit(EntityDto input)
        {
            var transactionBin = await _transactionBinRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTransactionBinForEditOutput { TransactionBin = ObjectMapper.Map<CreateOrEditTransactionBinDto>(transactionBin) };

            if (output.TransactionBin.DeviceId != null)
            {
                var _lookupDevice = await _lookup_deviceRepository.FirstOrDefaultAsync((int)output.TransactionBin.DeviceId);
                output.DeviceName = _lookupDevice?.Name?.ToString();
            }

            if (output.TransactionBin.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TransactionBin.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.TransactionBin.TransactionStatusId != null)
            {
                var _lookupTransactionStatus = await _lookup_transactionStatusRepository.FirstOrDefaultAsync((int)output.TransactionBin.TransactionStatusId);
                output.TransactionStatusName = _lookupTransactionStatus?.Name?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditTransactionBinDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionBins_Create), AbpAuthorize(AppPermissions.Pages_AdministrationDevice_TransactionBins_Create)]
        protected virtual async Task Create(CreateOrEditTransactionBinDto input)
        {
            var transactionBin = ObjectMapper.Map<TransactionBin>(input);
            var epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            transactionBin.TransactionCode = AppConsts.getCodeRandom(AppConsts.keyPerfixTransactionBins);
            await _transactionBinRepository.InsertAsync(transactionBin);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionBins_Edit), AbpAuthorize(AppPermissions.Pages_CustomerTransactionBins_Update)]
        protected virtual async Task Update(CreateOrEditTransactionBinDto input)
        {
            var transactionBin = await _transactionBinRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, transactionBin);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionBins_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _transactionBinRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetTransactionBinsToExcel(GetAllTransactionBinsForExcelInput input)
        {

            var filteredTransactionBins = _transactionBinRepository.GetAll()
                        .Include(e => e.DeviceFk)
                        .Include(e => e.UserFk)
                        .Include(e => e.TransactionStatusFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TransactionCode.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionCodeFilter), e => e.TransactionCode.Contains(input.TransactionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DeviceNameFilter), e => e.DeviceFk != null && e.DeviceFk.Name == input.DeviceNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TransactionStatusNameFilter), e => e.TransactionStatusFk != null && e.TransactionStatusFk.Name == input.TransactionStatusNameFilter);

            var query = (from o in filteredTransactionBins
                         join o1 in _lookup_deviceRepository.GetAll() on o.DeviceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_transactionStatusRepository.GetAll() on o.TransactionStatusId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetTransactionBinForViewDto()
                         {
                             TransactionBin = new TransactionBinDto
                             {
                                 PlastisQuantity = o.PlastisQuantity,
                                 PlastisPoint = o.PlastisPoint,
                                 MetalQuantity = o.MetalQuantity,
                                 MetalPoint = o.MetalPoint,
                                 OrtherQuantity = o.OrtherQuantity,
                                 ErrorPoint = o.ErrorPoint,
                                 TransactionCode = o.TransactionCode,
                                 Id = o.Id
                             },
                             DeviceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             TransactionStatusName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var transactionBinListDtos = await query.ToListAsync();

            return _transactionBinsExcelExporter.ExportToFile(transactionBinListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionBins)]
        public async Task<PagedResultDto<TransactionBinDeviceLookupTableDto>> GetAllDeviceForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_deviceRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var deviceList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TransactionBinDeviceLookupTableDto>();
            foreach (var device in deviceList)
            {
                lookupTableDtoList.Add(new TransactionBinDeviceLookupTableDto
                {
                    Id = device.Id,
                    DisplayName = device.Name?.ToString()
                });
            }

            return new PagedResultDto<TransactionBinDeviceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionBins)]
        public async Task<PagedResultDto<TransactionBinUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<TransactionBinUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new TransactionBinUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<TransactionBinUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionBins)]
        public async Task<List<TransactionBinTransactionStatusLookupTableDto>> GetAllTransactionStatusForTableDropdown()
        {
            return await _lookup_transactionStatusRepository.GetAll()
                .Select(transactionStatus => new TransactionBinTransactionStatusLookupTableDto
                {
                    Id = transactionStatus.Id,
                    DisplayName = transactionStatus == null || transactionStatus.Name == null ? "" : transactionStatus.Name.ToString()
                }).ToListAsync();
        }

    }
}