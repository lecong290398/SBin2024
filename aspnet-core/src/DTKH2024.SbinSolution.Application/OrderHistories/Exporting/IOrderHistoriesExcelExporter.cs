using System.Collections.Generic;
using DTKH2024.SbinSolution.OrderHistories.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.OrderHistories.Exporting
{
    public interface IOrderHistoriesExcelExporter
    {
        FileDto ExportToFile(List<GetOrderHistoryForViewDto> orderHistories);
    }
}