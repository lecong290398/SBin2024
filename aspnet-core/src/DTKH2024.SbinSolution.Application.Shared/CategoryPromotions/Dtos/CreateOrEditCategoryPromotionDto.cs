using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.CategoryPromotions.Dtos
{
    public class CreateOrEditCategoryPromotionDto : EntityDto<int?>
    {

        [Required]
        [StringLength(CategoryPromotionConsts.MaxNameLength, MinimumLength = CategoryPromotionConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

    }
}