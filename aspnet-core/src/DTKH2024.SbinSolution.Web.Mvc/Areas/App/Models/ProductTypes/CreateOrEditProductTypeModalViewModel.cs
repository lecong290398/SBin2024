using DTKH2024.SbinSolution.ProductTypes.Dtos;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.ProductTypes
{
    public class CreateOrEditProductTypeModalViewModel
    {
        public CreateOrEditProductTypeDto ProductType { get; set; }

        public bool IsEditMode => ProductType.Id.HasValue;
    }
}