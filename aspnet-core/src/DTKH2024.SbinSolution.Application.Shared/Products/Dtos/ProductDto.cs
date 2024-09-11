using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.Products.Dtos
{
    public class ProductDto : EntityDto
    {
        public string ProductName { get; set; }

        public string TimeDescription { get; set; }

        public string ApplicableSubjects { get; set; }

        public string Regulations { get; set; }

        public string UserManual { get; set; }

        public string ScopeOfApplication { get; set; }

        public string SupportAndComplaints { get; set; }

        public string Description { get; set; }

        public Guid? Image { get; set; }

        public string ImageFileName { get; set; }

        public int ProductTypeId { get; set; }

        public int BrandId { get; set; }

    }
}