using System.Collections.Generic;
using DTKH2024.SbinSolution.Devices.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Devices.Exporting
{
    public interface IDevicesExcelExporter
    {
        FileDto ExportToFile(List<GetDeviceForViewDto> devices);
    }
}