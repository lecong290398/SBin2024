using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.OrderHistories.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}