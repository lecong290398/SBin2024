using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.TransactionStatuses.Exporting;
using DTKH2024.SbinSolution.TransactionStatuses.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.TransactionStatuses
{
    [AbpAuthorize(AppPermissions.Pages_Administration_TransactionStatuses)]
    public class TransactionStatusesAppService : SbinSolutionAppServiceBase, ITransactionStatusesAppService
    {
        private readonly IRepository<TransactionStatus> _transactionStatusRepository;
        private readonly ITransactionStatusesExcelExporter _transactionStatusesExcelExporter;

        public TransactionStatusesAppService(IRepository<TransactionStatus> transactionStatusRepository, ITransactionStatusesExcelExporter transactionStatusesExcelExporter)
        {
            _transactionStatusRepository = transactionStatusRepository;
            _transactionStatusesExcelExporter = transactionStatusesExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetTransactionStatusForViewDto>> GetAll(GetAllTransactionStatusesInput input)
        {

            var filteredTransactionStatuses = _transactionStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredTransactionStatuses = filteredTransactionStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var transactionStatuses = from o in pagedAndFilteredTransactionStatuses
                                      select new
                                      {

                                          o.Name,
                                          o.Description,
                                          o.Color,
                                          Id = o.Id
                                      };

            var totalCount = await filteredTransactionStatuses.CountAsync();

            var dbList = await transactionStatuses.ToListAsync();
            var results = new List<GetTransactionStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTransactionStatusForViewDto()
                {
                    TransactionStatus = new TransactionStatusDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Color = o.Color,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTransactionStatusForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetTransactionStatusForViewDto> GetTransactionStatusForView(int id)
        {
            var transactionStatus = await _transactionStatusRepository.GetAsync(id);

            var output = new GetTransactionStatusForViewDto { TransactionStatus = ObjectMapper.Map<TransactionStatusDto>(transactionStatus) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionStatuses_Edit)]
        public virtual async Task<GetTransactionStatusForEditOutput> GetTransactionStatusForEdit(EntityDto input)
        {
            var transactionStatus = await _transactionStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTransactionStatusForEditOutput { TransactionStatus = ObjectMapper.Map<CreateOrEditTransactionStatusDto>(transactionStatus) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditTransactionStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionStatuses_Create)]
        protected virtual async Task Create(CreateOrEditTransactionStatusDto input)
        {
            var transactionStatus = ObjectMapper.Map<TransactionStatus>(input);

            await _transactionStatusRepository.InsertAsync(transactionStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditTransactionStatusDto input)
        {
            var transactionStatus = await _transactionStatusRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, transactionStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_TransactionStatuses_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _transactionStatusRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetTransactionStatusesToExcel(GetAllTransactionStatusesForExcelInput input)
        {

            var filteredTransactionStatuses = _transactionStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredTransactionStatuses
                         select new GetTransactionStatusForViewDto()
                         {
                             TransactionStatus = new TransactionStatusDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Color = o.Color,
                                 Id = o.Id
                             }
                         });

            var transactionStatusListDtos = await query.ToListAsync();

            return _transactionStatusesExcelExporter.ExportToFile(transactionStatusListDtos);
        }

    }
}