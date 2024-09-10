using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.StatusDevices.Dtos
{
    public class StatusDeviceDto : EntityDto
    {
        public string Name { get; set; }

        public string Color { get; set; }

    }
}