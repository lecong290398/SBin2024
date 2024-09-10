using System.Collections.Generic;
using DTKH2024.SbinSolution.DynamicEntityProperties.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.DynamicEntityProperty
{
    public class CreateEntityDynamicPropertyViewModel
    {
        public string EntityFullName { get; set; }

        public List<string> AllEntities { get; set; }

        public List<DynamicPropertyDto> DynamicProperties { get; set; }
    }
}
