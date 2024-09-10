using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Organizations.Dto
{
    public class FindOrganizationUnitRolesInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}