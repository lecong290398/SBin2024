using DTKH2024.SbinSolution.StatusDevices;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.Devices
{
    [Table("Devices")]
    public class Device : FullAuditedEntity
    {

        public virtual string Name { get; set; }

        public virtual decimal PlastisPoint { get; set; }

        public virtual bool SensorPlastisAvailable { get; set; }

        public virtual int PercentStatusPlastis { get; set; }

        public virtual decimal MetalPoint { get; set; }

        public virtual bool SensorMetalAvailable { get; set; }

        public virtual int PercentStatusMetal { get; set; }

        public virtual int PercentStatusOrther { get; set; }

        public virtual decimal ErrorPoint { get; set; }

        public virtual string Address { get; set; }

        public virtual int StatusDeviceId { get; set; }

        [ForeignKey("StatusDeviceId")]
        public StatusDevice StatusDeviceFk { get; set; }

    }
}