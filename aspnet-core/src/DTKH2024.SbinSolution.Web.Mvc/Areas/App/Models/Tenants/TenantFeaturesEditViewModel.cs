﻿using Abp.AutoMapper;
using DTKH2024.SbinSolution.MultiTenancy;
using DTKH2024.SbinSolution.MultiTenancy.Dto;
using DTKH2024.SbinSolution.Web.Areas.App.Models.Common;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}