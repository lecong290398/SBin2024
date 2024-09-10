using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.Brands
{
    [Table("Brands")]
    public class Brand : FullAuditedEntity
    {

        [Required]
        [StringLength(BrandConsts.MaxNameLength, MinimumLength = BrandConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(BrandConsts.MaxDescriptionLength, MinimumLength = BrandConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }
        //File

        public virtual Guid? Logo { get; set; } //File, (BinaryObjectId)

    }
}