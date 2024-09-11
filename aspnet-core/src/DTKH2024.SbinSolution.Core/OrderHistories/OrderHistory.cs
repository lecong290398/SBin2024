using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.TransactionBins;
using DTKH2024.SbinSolution.WareHouseGifts;
using DTKH2024.SbinSolution.HistoryTypes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.OrderHistories
{
    [Table("OrderHistories")]
    public class OrderHistory : CreationAuditedEntity
    {

        public virtual string Description { get; set; }

        public virtual string Reason { get; set; }

        public virtual int Point { get; set; }

        public virtual long UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual int? TransactionBinId { get; set; }

        [ForeignKey("TransactionBinId")]
        public TransactionBin TransactionBinFk { get; set; }

        public virtual int? WareHouseGiftId { get; set; }

        [ForeignKey("WareHouseGiftId")]
        public WareHouseGift WareHouseGiftFk { get; set; }

        public virtual int HistoryTypeId { get; set; }

        [ForeignKey("HistoryTypeId")]
        public HistoryType HistoryTypeFk { get; set; }

    }
}