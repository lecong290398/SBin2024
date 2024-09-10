using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.RankLevels.Dtos
{
    public class CreateOrEditRankLevelDto : EntityDto<int?>
    {

        [StringLength(RankLevelConsts.MaxNameLength, MinimumLength = RankLevelConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal MinimumPositiveScore { get; set; }

        public string Color { get; set; }

        public Guid? Logo { get; set; }

        public string LogoToken { get; set; }

    }
}