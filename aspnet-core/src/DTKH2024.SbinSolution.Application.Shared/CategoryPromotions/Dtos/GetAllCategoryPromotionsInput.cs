using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.CategoryPromotions.Dtos
{
    public class GetAllCategoryPromotionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string ColorFilter { get; set; }

    }
}