using System.Collections.Generic;
using DTKH2024.SbinSolution.TransactionBins.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.TransactionBins.Exporting
{
    public interface ITransactionBinsExcelExporter
    {
        FileDto ExportToFile(List<GetTransactionBinForViewDto> transactionBins);
    }
}