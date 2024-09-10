using System.Collections.Generic;
using DTKH2024.SbinSolution.HistoryTypes.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.HistoryTypes.Exporting
{
    public interface IHistoryTypesExcelExporter
    {
        FileDto ExportToFile(List<GetHistoryTypeForViewDto> historyTypes);
    }
}