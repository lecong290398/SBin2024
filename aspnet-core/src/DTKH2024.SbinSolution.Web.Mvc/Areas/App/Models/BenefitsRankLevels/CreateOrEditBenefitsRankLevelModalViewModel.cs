using DTKH2024.SbinSolution.BenefitsRankLevels.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.BenefitsRankLevels
{
    public class CreateOrEditBenefitsRankLevelModalViewModel
    {
        public CreateOrEditBenefitsRankLevelDto BenefitsRankLevel { get; set; }

        public string RankLevelName { get; set; }

        public List<BenefitsRankLevelRankLevelLookupTableDto> BenefitsRankLevelRankLevelList { get; set; }

        public bool IsEditMode => BenefitsRankLevel.Id.HasValue;
    }
}