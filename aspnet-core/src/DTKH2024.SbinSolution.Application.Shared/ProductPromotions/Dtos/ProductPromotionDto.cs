using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.ProductPromotions.Dtos
{
    public class ProductPromotionDto : EntityDto
    {
        public int Point { get; set; }

        public int QuantityCurrent { get; set; }

        public int QuantityInStock { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string PromotionCode { get; set; }

        public string Description { get; set; }

        public int ProductId { get; set; }

        public int CategoryPromotionId { get; set; }

    }
}