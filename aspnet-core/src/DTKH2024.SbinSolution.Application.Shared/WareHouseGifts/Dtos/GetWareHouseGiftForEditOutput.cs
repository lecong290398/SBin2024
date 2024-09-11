using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace DTKH2024.SbinSolution.WareHouseGifts.Dtos
{
    public class GetWareHouseGiftForEditOutput
    {
        public CreateOrEditWareHouseGiftDto WareHouseGift { get; set; }

        public string UserName { get; set; }

        public string ProductPromotionPromotionCode { get; set; }

    }
}