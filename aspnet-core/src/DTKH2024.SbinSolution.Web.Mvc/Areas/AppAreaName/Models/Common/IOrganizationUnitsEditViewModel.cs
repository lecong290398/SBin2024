using System.Collections.Generic;
using DTKH2024.SbinSolution.Organizations.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        List<string> MemberedOrganizationUnits { get; set; }
    }
}