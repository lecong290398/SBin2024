using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.ProductPromotions.Dtos
{
    public class GetAllProductPromotionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string PromotionNameFilter { get; set; }

        public int? MaxPointFilter { get; set; }
        public int? MinPointFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public string ProductProductNameFilter { get; set; }

    }
}