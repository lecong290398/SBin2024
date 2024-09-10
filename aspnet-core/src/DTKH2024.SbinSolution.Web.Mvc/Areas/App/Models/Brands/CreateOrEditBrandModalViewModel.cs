using DTKH2024.SbinSolution.Brands.Dtos;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Brands
{
    public class CreateOrEditBrandModalViewModel
    {
        public CreateOrEditBrandDto Brand { get; set; }

        public string LogoFileName { get; set; }
        public string LogoFileAcceptedTypes { get; set; }

        public bool IsEditMode => Brand.Id.HasValue;
    }
}