using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.OrderHistories.Dtos;
using DTKH2024.SbinSolution.Dto;
using System.Collections.Generic;

namespace DTKH2024.SbinSolution.OrderHistories
{
    public interface IOrderHistoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderHistoryForViewDto>> GetAll(GetAllOrderHistoriesInput input);

        Task<GetOrderHistoryForViewDto> GetOrderHistoryForView(int id);

        Task<GetOrderHistoryForEditOutput> GetOrderHistoryForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOrderHistoryDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetOrderHistoriesToExcel(GetAllOrderHistoriesForExcelInput input);

        Task<PagedResultDto<OrderHistoryUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderHistoryTransactionBinLookupTableDto>> GetAllTransactionBinForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<OrderHistoryWareHouseGiftLookupTableDto>> GetAllWareHouseGiftForLookupTable(GetAllForLookupTableInput input);

        Task<List<OrderHistoryHistoryTypeLookupTableDto>> GetAllHistoryTypeForTableDropdown();

    }
}