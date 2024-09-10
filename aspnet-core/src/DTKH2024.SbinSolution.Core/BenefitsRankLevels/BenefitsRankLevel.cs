using DTKH2024.SbinSolution.RankLevels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace DTKH2024.SbinSolution.BenefitsRankLevels
{
    [Table("BenefitsRankLevels")]
    public class BenefitsRankLevel : FullAuditedEntity
    {

        [StringLength(BenefitsRankLevelConsts.MaxNameLength, MinimumLength = BenefitsRankLevelConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual int RankLevelId { get; set; }

        [ForeignKey("RankLevelId")]
        public RankLevel RankLevelFk { get; set; }

    }
}