using Abp.AspNetCore.Mvc.Views;

namespace DTKH2024.SbinSolution.Web.Views
{
    public abstract class SbinSolutionRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected SbinSolutionRazorPage()
        {
            LocalizationSourceName = SbinSolutionConsts.LocalizationSourceName;
        }
    }
}
