using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.StatusDevices.Dtos
{
    public class GetStatusDeviceForEditOutput
    {
        public CreateOrEditStatusDeviceDto StatusDevice { get; set; }

    }
}