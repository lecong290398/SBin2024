using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.OrderHistories.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.OrderHistories.Exporting
{
    public class OrderHistoriesExcelExporter : MiniExcelExcelExporterBase, IOrderHistoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrderHistoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrderHistoryForViewDto> orderHistories)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var orderHistory in orderHistories)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Description"), orderHistory.OrderHistory.Description},
                        {L("Reason"), orderHistory.OrderHistory.Reason},
                        {L("Point"), orderHistory.OrderHistory.Point},

                    });
            }

            return CreateExcelPackage("OrderHistoriesList.xlsx", items);

        }
    }
}