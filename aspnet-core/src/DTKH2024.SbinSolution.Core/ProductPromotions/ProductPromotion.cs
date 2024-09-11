using DTKH2024.SbinSolution.Products;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.ProductPromotions
{
    [Table("ProductPromotions")]
    public class ProductPromotion : FullAuditedEntity
    {

        public virtual int Point { get; set; }

        public virtual int QuantityCurrent { get; set; }

        public virtual int QuantityInStock { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual string PromotionCode { get; set; }

        public virtual string Description { get; set; }

        public virtual int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product ProductFk { get; set; }

    }
}