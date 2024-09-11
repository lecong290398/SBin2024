using DTKH2024.SbinSolution.ProductPromotions.Dtos;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.ProductPromotions
{
    public class CreateOrEditProductPromotionModalViewModel
    {
        public CreateOrEditProductPromotionDto ProductPromotion { get; set; }

        public string ProductProductName { get; set; }

        public string CategoryPromotionName { get; set; }

        public bool IsEditMode => ProductPromotion.Id.HasValue;
    }
}