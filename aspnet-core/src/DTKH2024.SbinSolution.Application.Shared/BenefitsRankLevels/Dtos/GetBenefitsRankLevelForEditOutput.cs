using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.BenefitsRankLevels.Dtos
{
    public class GetBenefitsRankLevelForEditOutput
    {
        public CreateOrEditBenefitsRankLevelDto BenefitsRankLevel { get; set; }

        public string RankLevelName { get; set; }

    }
}