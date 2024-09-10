using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.Devices.Dtos
{
    public class GetAllDevicesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? SensorPlastisAvailableFilter { get; set; }

        public int? SensorMetalAvailableFilter { get; set; }

        public string StatusDeviceNameFilter { get; set; }

    }
}