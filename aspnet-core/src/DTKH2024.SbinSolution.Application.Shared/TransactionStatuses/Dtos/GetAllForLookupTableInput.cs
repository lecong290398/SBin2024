using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.TransactionStatuses.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}