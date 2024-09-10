using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using DTKH2024.SbinSolution.Editions.Dto;
using DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Common;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Editions
{
    [AutoMapFrom(typeof(GetEditionEditOutput))]
    public class CreateEditionModalViewModel : GetEditionEditOutput, IFeatureEditViewModel
    {
        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }

        public IReadOnlyList<ComboboxItemDto> FreeEditionItems { get; set; }
    }
}