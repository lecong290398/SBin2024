using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.StatusDevices.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.StatusDevices.Exporting
{
    public class StatusDevicesExcelExporter : MiniExcelExcelExporterBase, IStatusDevicesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StatusDevicesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStatusDeviceForViewDto> statusDevices)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var statusDevice in statusDevices)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Name"), statusDevice.StatusDevice.Name},
                        {L("Color"), statusDevice.StatusDevice.Color},

                    });
            }

            return CreateExcelPackage("StatusDevicesList.xlsx", items);

        }
    }
}