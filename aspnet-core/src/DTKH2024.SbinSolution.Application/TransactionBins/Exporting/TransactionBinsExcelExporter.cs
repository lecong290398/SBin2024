using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.TransactionBins.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.TransactionBins.Exporting
{
    public class TransactionBinsExcelExporter : MiniExcelExcelExporterBase, ITransactionBinsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransactionBinsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTransactionBinForViewDto> transactionBins)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var transactionBin in transactionBins)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("PlastisQuantity"), transactionBin.TransactionBin.PlastisQuantity},
                        {L("PlastisPoint"), transactionBin.TransactionBin.PlastisPoint},
                        {L("MetalQuantity"), transactionBin.TransactionBin.MetalQuantity},
                        {L("MetalPoint"), transactionBin.TransactionBin.MetalPoint},
                        {L("OrtherQuantity"), transactionBin.TransactionBin.OrtherQuantity},
                        {L("ErrorPoint"), transactionBin.TransactionBin.ErrorPoint},
                        {L("TransactionCode"), transactionBin.TransactionBin.TransactionCode},

                    });
            }

            return CreateExcelPackage("TransactionBinsList.xlsx", items);

        }
    }
}