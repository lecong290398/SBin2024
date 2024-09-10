using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.HistoryTypes.Dtos
{
    public class GetAllHistoryTypesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}