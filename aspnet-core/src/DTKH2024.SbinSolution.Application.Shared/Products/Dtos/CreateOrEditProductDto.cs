using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.Products.Dtos
{
    public class CreateOrEditProductDto : EntityDto<int?>
    {

        [Required]
        [StringLength(ProductConsts.MaxProductNameLength, MinimumLength = ProductConsts.MinProductNameLength)]
        public string ProductName { get; set; }

        public string TimeDescription { get; set; }

        public string ApplicableSubjects { get; set; }

        public string Regulations { get; set; }

        public string UserManual { get; set; }

        public string ScopeOfApplication { get; set; }

        public string SupportAndComplaints { get; set; }

        public string Description { get; set; }

        public Guid? Image { get; set; }

        public string ImageToken { get; set; }

        public int ProductTypeId { get; set; }

        public int BrandId { get; set; }

    }
}