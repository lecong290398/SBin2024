using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.Devices.Dtos
{
    public class DeviceDto : EntityDto
    {
        public string Name { get; set; }

        public decimal PlastisPoint { get; set; }

        public bool SensorPlastisAvailable { get; set; }

        public int PercentStatusPlastis { get; set; }

        public decimal MetalPoint { get; set; }

        public bool SensorMetalAvailable { get; set; }

        public int PercentStatusMetal { get; set; }

        public int PercentStatusOrther { get; set; }

        public decimal ErrorPoint { get; set; }

        public string Address { get; set; }

        public int StatusDeviceId { get; set; }

        public long? UserId { get; set; }
        public string LastModificationTime { get; set; }

    }
}