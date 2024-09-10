using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.BenefitsRankLevels.Dtos
{
    public class CreateOrEditBenefitsRankLevelDto : EntityDto<int?>
    {

        [StringLength(BenefitsRankLevelConsts.MaxNameLength, MinimumLength = BenefitsRankLevelConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int RankLevelId { get; set; }

    }
}