using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.TransactionStatuses.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.TransactionStatuses.Exporting
{
    public class TransactionStatusesExcelExporter : MiniExcelExcelExporterBase, ITransactionStatusesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransactionStatusesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTransactionStatusForViewDto> transactionStatuses)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var transactionStatus in transactionStatuses)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Name"), transactionStatus.TransactionStatus.Name},
                        {L("Description"), transactionStatus.TransactionStatus.Description},
                        {L("Color"), transactionStatus.TransactionStatus.Color},

                    });
            }

            return CreateExcelPackage("TransactionStatusesList.xlsx", items);

        }
    }
}