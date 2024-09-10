using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.RankLevels.Dtos
{
    public class RankLevelDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal MinimumPositiveScore { get; set; }

        public string Color { get; set; }

        public Guid? Logo { get; set; }

        public string LogoFileName { get; set; }

    }
}