using DTKH2024.SbinSolution.CategoryPromotions.Dtos;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.CategoryPromotions
{
    public class CreateOrEditCategoryPromotionModalViewModel
    {
        public CreateOrEditCategoryPromotionDto CategoryPromotion { get; set; }

        public bool IsEditMode => CategoryPromotion.Id.HasValue;
    }
}