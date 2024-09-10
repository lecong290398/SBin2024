using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public int? TenantId { get; set; }

        public bool ExcludeCurrentUser { get; set; }
    }
}