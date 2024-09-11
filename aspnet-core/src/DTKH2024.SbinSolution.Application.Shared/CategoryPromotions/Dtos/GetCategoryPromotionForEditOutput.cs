using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.CategoryPromotions.Dtos
{
    public class GetCategoryPromotionForEditOutput
    {
        public CreateOrEditCategoryPromotionDto CategoryPromotion { get; set; }

    }
}