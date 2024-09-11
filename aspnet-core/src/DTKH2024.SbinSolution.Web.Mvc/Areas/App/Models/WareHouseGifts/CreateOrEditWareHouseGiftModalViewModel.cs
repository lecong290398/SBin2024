using DTKH2024.SbinSolution.WareHouseGifts.Dtos;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.WareHouseGifts
{
    public class CreateOrEditWareHouseGiftModalViewModel
    {
        public CreateOrEditWareHouseGiftDto WareHouseGift { get; set; }

        public string UserName { get; set; }

        public string ProductPromotionPromotionCode { get; set; }

        public bool IsEditMode => WareHouseGift.Id.HasValue;
    }
}