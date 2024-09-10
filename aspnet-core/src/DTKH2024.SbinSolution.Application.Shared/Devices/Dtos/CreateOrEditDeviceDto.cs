using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.Devices.Dtos
{
    public class CreateOrEditDeviceDto : EntityDto<int?>
    {

        public string Name { get; set; }

        public decimal PlastisPoint { get; set; }

        public bool SensorPlastisAvailable { get; set; }

        public int PercentStatusPlastis { get; set; }

        public decimal MetalPoint { get; set; }

        public bool SensorMetalAvailable { get; set; }

        public int PercentStatusMetal { get; set; }

        public decimal ErrorPoint { get; set; }

        public string Address { get; set; }

        public int StatusDeviceId { get; set; }

    }
}