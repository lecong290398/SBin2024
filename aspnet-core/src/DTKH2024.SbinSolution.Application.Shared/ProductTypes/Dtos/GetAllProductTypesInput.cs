using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.ProductTypes.Dtos
{
    public class GetAllProductTypesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}