using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.StatusDevices.Dtos
{
    public class GetAllStatusDevicesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}