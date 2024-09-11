using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.Products.Dtos
{
    public class GetAllProductsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ProductTypeNameFilter { get; set; }

        public string BrandNameFilter { get; set; }

    }
}