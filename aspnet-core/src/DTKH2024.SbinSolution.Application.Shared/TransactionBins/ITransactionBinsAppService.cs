using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.TransactionBins.Dtos;
using DTKH2024.SbinSolution.Dto;
using System.Collections.Generic;

namespace DTKH2024.SbinSolution.TransactionBins
{
    public interface ITransactionBinsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTransactionBinForViewDto>> GetAll(GetAllTransactionBinsInput input);

        Task<GetTransactionBinForViewDto> GetTransactionBinForView(int id);

        Task<GetTransactionBinForEditOutput> GetTransactionBinForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTransactionBinDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTransactionBinsToExcel(GetAllTransactionBinsForExcelInput input);

        Task<PagedResultDto<TransactionBinDeviceLookupTableDto>> GetAllDeviceForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<TransactionBinUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<List<TransactionBinTransactionStatusLookupTableDto>> GetAllTransactionStatusForTableDropdown();

        Task<string> CreateDevice_TransactionBins(CreateTransactionDeviceBinDto input);

    }
}