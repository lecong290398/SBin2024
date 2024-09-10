using Abp.AutoMapper;
using DTKH2024.SbinSolution.Localization.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;
    }
}