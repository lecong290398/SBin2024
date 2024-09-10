using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.Devices.Dtos
{
    public class GetDeviceForEditOutput
    {
        public CreateOrEditDeviceDto Device { get; set; }

        public string StatusDeviceName { get; set; }

    }
}