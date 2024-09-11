using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.ProductPromotions.Dtos
{
    public class CreateOrEditProductPromotionDto : EntityDto<int?>
    {

        [Required]
        [StringLength(ProductPromotionConsts.MaxPromotionNameLength, MinimumLength = ProductPromotionConsts.MinPromotionNameLength)]
        public string PromotionName { get; set; }

        public int Point { get; set; }

        public int QuantityCurrent { get; set; }

        public int QuantityInStock { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int ProductId { get; set; }

    }
}