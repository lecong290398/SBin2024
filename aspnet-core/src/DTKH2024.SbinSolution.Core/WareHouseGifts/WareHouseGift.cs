using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.ProductPromotions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.WareHouseGifts
{
    [Table("WareHouseGifts")]
    public class WareHouseGift : FullAuditedEntity
    {

        [Required]
        public virtual string Code { get; set; }

        public virtual bool IsUsed { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual int? ProductPromotionId { get; set; }

        [ForeignKey("ProductPromotionId")]
        public ProductPromotion ProductPromotionFk { get; set; }

    }
}