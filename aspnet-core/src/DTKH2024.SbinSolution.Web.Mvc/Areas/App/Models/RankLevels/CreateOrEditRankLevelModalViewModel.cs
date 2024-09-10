using DTKH2024.SbinSolution.RankLevels.Dtos;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.RankLevels
{
    public class CreateOrEditRankLevelModalViewModel
    {
        public CreateOrEditRankLevelDto RankLevel { get; set; }

        public string LogoFileName { get; set; }
        public string LogoFileAcceptedTypes { get; set; }

        public bool IsEditMode => RankLevel.Id.HasValue;
    }
}