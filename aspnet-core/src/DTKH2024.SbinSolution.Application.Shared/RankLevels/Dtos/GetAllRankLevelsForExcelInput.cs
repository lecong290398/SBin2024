using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.RankLevels.Dtos
{
    public class GetAllRankLevelsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}