using System.Collections.Generic;
using DTKH2024.SbinSolution.TransactionStatuses.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.TransactionStatuses.Exporting
{
    public interface ITransactionStatusesExcelExporter
    {
        FileDto ExportToFile(List<GetTransactionStatusForViewDto> transactionStatuses);
    }
}