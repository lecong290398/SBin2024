using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.Editions.Dto
{
    public class EditionWithFeaturesDto
    {
        public EditionWithFeaturesDto()
        {
            FeatureValues = new List<NameValueDto>();
        }

        public EditionSelectDto Edition { get; set; }

        public List<NameValueDto> FeatureValues { get; set; }
    }
}