using System.Collections.Generic;
using DTKH2024.SbinSolution.Organizations.Dto;
using DTKH2024.SbinSolution.Web.Areas.App.Models.Common;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.OrganizationUnits
{
    public class OrganizationUnitLookupTableModel : IOrganizationUnitsEditViewModel
    {
        public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }
        
        public List<string> MemberedOrganizationUnits { get; set; }

        public OrganizationUnitLookupTableModel()
        {
            AllOrganizationUnits = new List<OrganizationUnitDto>();
            MemberedOrganizationUnits = new List<string>();
        }
    }
}