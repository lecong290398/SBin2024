﻿using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.BenefitsRankLevels.Dtos
{
    public class GetAllBenefitsRankLevelsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string RankLevelNameFilter { get; set; }

    }
}