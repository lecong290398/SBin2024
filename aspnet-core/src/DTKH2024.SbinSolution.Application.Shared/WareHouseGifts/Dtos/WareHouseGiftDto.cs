using System;
using Abp.Application.Services.Dto;

namespace DTKH2024.SbinSolution.WareHouseGifts.Dtos
{
    public class WareHouseGiftDto : EntityDto
    {
        public string Code { get; set; }
        public string CreationTime { get; set; }
        public string LastModificationTime { get; set; }

        public bool IsUsed { get; set; }

        public long UserId { get; set; }

        public int? ProductPromotionId { get; set; }

    }
}