using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Editions.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}