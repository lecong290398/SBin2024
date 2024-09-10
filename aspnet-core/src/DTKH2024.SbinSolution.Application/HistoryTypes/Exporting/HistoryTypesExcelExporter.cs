using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.HistoryTypes.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.HistoryTypes.Exporting
{
    public class HistoryTypesExcelExporter : MiniExcelExcelExporterBase, IHistoryTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public HistoryTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetHistoryTypeForViewDto> historyTypes)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var historyType in historyTypes)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Name"), historyType.HistoryType.Name},
                        {L("Description"), historyType.HistoryType.Description},
                        {L("Color"), historyType.HistoryType.Color},

                    });
            }

            return CreateExcelPackage("HistoryTypesList.xlsx", items);

        }
    }
}