using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.RankLevels
{
    [Table("RankLevels")]
    public class RankLevel : FullAuditedEntity
    {

        [StringLength(RankLevelConsts.MaxNameLength, MinimumLength = RankLevelConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual decimal MinimumPositiveScore { get; set; }

        public virtual string Color { get; set; }
        //File

        public virtual Guid? Logo { get; set; } //File, (BinaryObjectId)

    }
}