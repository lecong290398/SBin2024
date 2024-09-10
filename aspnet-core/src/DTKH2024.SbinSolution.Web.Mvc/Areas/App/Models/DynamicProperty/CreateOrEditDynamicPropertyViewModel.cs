using System.Collections.Generic;
using DTKH2024.SbinSolution.DynamicEntityProperties.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.DynamicProperty
{
    public class CreateOrEditDynamicPropertyViewModel
    {
        public DynamicPropertyDto DynamicPropertyDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
