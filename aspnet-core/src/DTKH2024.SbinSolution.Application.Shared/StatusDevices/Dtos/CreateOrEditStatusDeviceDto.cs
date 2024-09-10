using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.StatusDevices.Dtos
{
    public class CreateOrEditStatusDeviceDto : EntityDto<int?>
    {

        [Required]
        [StringLength(StatusDeviceConsts.MaxNameLength, MinimumLength = StatusDeviceConsts.MinNameLength)]
        public string Name { get; set; }

        public string Color { get; set; }

    }
}