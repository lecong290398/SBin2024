using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.BenefitsRankLevels.Dtos
{
    public class BenefitsRankLevelDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int RankLevelId { get; set; }

    }
}