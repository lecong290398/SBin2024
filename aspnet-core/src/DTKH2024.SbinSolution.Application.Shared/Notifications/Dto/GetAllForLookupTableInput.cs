using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.Notifications.Dto
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}