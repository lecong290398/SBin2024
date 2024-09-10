using Abp.AspNetCore.Mvc.ViewComponents;

namespace DTKH2024.SbinSolution.Web.Views
{
    public abstract class SbinSolutionViewComponent : AbpViewComponent
    {
        protected SbinSolutionViewComponent()
        {
            LocalizationSourceName = SbinSolutionConsts.LocalizationSourceName;
        }
    }
}