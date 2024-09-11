using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.ProductPromotions.Dtos
{
    public class GetProductPromotionForEditOutput
    {
        public CreateOrEditProductPromotionDto ProductPromotion { get; set; }

        public string ProductProductName { get; set; }

        public string CategoryPromotionName { get; set; }

    }
}