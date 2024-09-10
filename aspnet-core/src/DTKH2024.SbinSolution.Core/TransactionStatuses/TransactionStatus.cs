using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.TransactionStatuses
{
    [Table("TransactionStatuses")]
    public class TransactionStatus : FullAuditedEntity
    {

        [StringLength(TransactionStatusConsts.MaxNameLength, MinimumLength = TransactionStatusConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string Color { get; set; }

    }
}