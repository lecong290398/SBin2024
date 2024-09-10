using Abp.AutoMapper;
using DTKH2024.SbinSolution.Organizations.Dto;

namespace DTKH2024.SbinSolution.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}
