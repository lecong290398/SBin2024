using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.Devices.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.Devices.Exporting
{
    public class DevicesExcelExporter : MiniExcelExcelExporterBase, IDevicesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DevicesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDeviceForViewDto> devices)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var device in devices)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Name"), device.Device.Name},
                        {L("PlastisPoint"), device.Device.PlastisPoint},
                        {L("SensorPlastisAvailable"), device.Device.SensorPlastisAvailable},
                        {L("PercentStatusPlastis"), device.Device.PercentStatusPlastis},
                        {L("MetalPoint"), device.Device.MetalPoint},
                        {L("SensorMetalAvailable"), device.Device.SensorMetalAvailable},
                        {L("PercentStatusMetal"), device.Device.PercentStatusMetal},
                        {L("ErrorPoint"), device.Device.ErrorPoint},
                        {L("Address"), device.Device.Address},

                    });
            }

            return CreateExcelPackage("DevicesList.xlsx", items);

        }
    }
}