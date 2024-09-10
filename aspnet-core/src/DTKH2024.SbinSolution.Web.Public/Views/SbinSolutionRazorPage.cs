﻿using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace DTKH2024.SbinSolution.Web.Public.Views
{
    public abstract class SbinSolutionRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected SbinSolutionRazorPage()
        {
            LocalizationSourceName = SbinSolutionConsts.LocalizationSourceName;
        }
    }
}
