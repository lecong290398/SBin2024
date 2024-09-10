using System.Collections.Generic;
using DTKH2024.SbinSolution.StatusDevices.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.StatusDevices.Exporting
{
    public interface IStatusDevicesExcelExporter
    {
        FileDto ExportToFile(List<GetStatusDeviceForViewDto> statusDevices);
    }
}