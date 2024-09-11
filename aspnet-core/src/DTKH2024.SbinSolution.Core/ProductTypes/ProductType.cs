using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.ProductTypes
{
    [Table("ProductTypes")]
    public class ProductType : FullAuditedEntity
    {

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Color { get; set; }

    }
}