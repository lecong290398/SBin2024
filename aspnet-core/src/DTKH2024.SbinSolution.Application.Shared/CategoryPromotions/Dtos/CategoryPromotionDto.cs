using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.CategoryPromotions.Dtos
{
    public class CategoryPromotionDto : EntityDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

    }
}