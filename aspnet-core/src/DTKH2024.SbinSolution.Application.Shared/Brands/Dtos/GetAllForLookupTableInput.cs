using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.Brands.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}