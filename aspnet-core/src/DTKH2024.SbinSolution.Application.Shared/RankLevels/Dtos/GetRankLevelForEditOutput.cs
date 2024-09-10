using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.RankLevels.Dtos
{
    public class GetRankLevelForEditOutput
    {
        public CreateOrEditRankLevelDto RankLevel { get; set; }

        public string LogoFileName { get; set; }

    }
}