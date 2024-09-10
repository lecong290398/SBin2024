using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.Brands.Dtos
{
    public class CreateOrEditBrandDto : EntityDto<int?>
    {

        [Required]
        [StringLength(BrandConsts.MaxNameLength, MinimumLength = BrandConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(BrandConsts.MaxDescriptionLength, MinimumLength = BrandConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public Guid? Logo { get; set; }

        public string LogoToken { get; set; }

    }
}