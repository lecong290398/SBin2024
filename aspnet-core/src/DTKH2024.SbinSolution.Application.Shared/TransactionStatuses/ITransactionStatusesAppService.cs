using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.TransactionStatuses.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.TransactionStatuses
{
    public interface ITransactionStatusesAppService : IApplicationService
    {
        Task<PagedResultDto<GetTransactionStatusForViewDto>> GetAll(GetAllTransactionStatusesInput input);

        Task<GetTransactionStatusForViewDto> GetTransactionStatusForView(int id);

        Task<GetTransactionStatusForEditOutput> GetTransactionStatusForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTransactionStatusDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTransactionStatusesToExcel(GetAllTransactionStatusesForExcelInput input);

    }
}