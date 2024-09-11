using DTKH2024.SbinSolution.Devices;
using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.TransactionStatuses;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.TransactionBins
{
    [Table("TransactionBins")]
    public class TransactionBin : FullAuditedEntity
    {

        public virtual int PlastisQuantity { get; set; }

        public virtual decimal PlastisPoint { get; set; }

        public virtual int MetalQuantity { get; set; }

        public virtual decimal MetalPoint { get; set; }

        public virtual int OrtherQuantity { get; set; }

        public virtual decimal ErrorPoint { get; set; }

        public virtual string TransactionCode { get; set; }

        public virtual int DeviceId { get; set; }

        [ForeignKey("DeviceId")]
        public Device DeviceFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual int TransactionStatusId { get; set; }

        [ForeignKey("TransactionStatusId")]
        public TransactionStatus TransactionStatusFk { get; set; }

    }
}