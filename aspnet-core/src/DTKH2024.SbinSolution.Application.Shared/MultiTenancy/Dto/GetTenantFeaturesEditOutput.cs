using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Editions.Dto;

namespace DTKH2024.SbinSolution.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}