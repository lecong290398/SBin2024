using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.WareHouseGifts.Dtos
{
    public class CreateOrEditWareHouseGiftDto : EntityDto<int?>
    {

        public bool IsUsed { get; set; }

        public long UserId { get; set; }

        public int? ProductPromotionId { get; set; }

    }
}