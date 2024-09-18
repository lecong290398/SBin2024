using Abp.Application.Services.Dto;
using System;

namespace DTKH2024.SbinSolution.ProductPromotions.Dtos
{
    public class GetAllProductPromotionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MaxPointFilter { get; set; }
        public int? MinPointFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public string PromotionCodeFilter { get; set; }

        public string ProductProductNameFilter { get; set; }

        public string CategoryPromotionNameFilter { get; set; }

    }

    public class GetAllProductPromotionsInputForCustomer : GetAllProductPromotionsInput
    {
        public int? BrandID { get; set; }
    }


    public class GetProductPromotionsInputForCustomer 
    {
        public int ProductID { get; set; }
        public int ProductPromotionID { get; set; }
    }
}