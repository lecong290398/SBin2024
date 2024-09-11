using DTKH2024.SbinSolution.ProductTypes;
using DTKH2024.SbinSolution.Brands;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.Products
{
    [Table("Products")]
    public class Product : FullAuditedEntity
    {

        [Required]
        [StringLength(ProductConsts.MaxProductNameLength, MinimumLength = ProductConsts.MinProductNameLength)]
        public virtual string ProductName { get; set; }

        public virtual string TimeDescription { get; set; }

        public virtual string ApplicableSubjects { get; set; }

        public virtual string Regulations { get; set; }

        public virtual string UserManual { get; set; }

        public virtual string ScopeOfApplication { get; set; }

        public virtual string SupportAndComplaints { get; set; }

        public virtual string Description { get; set; }
        //File

        public virtual Guid? Image { get; set; } //File, (BinaryObjectId)

        public virtual int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public ProductType ProductTypeFk { get; set; }

        public virtual int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand BrandFk { get; set; }

    }
}