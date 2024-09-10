using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.StatusDevices
{
    [Table("StatusDevices")]
    public class StatusDevice : FullAuditedEntity
    {

        [Required]
        [StringLength(StatusDeviceConsts.MaxNameLength, MinimumLength = StatusDeviceConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Color { get; set; }

    }
}