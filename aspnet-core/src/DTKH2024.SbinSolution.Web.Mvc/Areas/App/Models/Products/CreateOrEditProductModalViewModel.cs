using DTKH2024.SbinSolution.Products.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Models.Products
{
    public class CreateOrEditProductModalViewModel
    {
        public CreateOrEditProductDto Product { get; set; }

        public string ProductTypeName { get; set; }

        public string BrandName { get; set; }

        public List<ProductProductTypeLookupTableDto> ProductProductTypeList { get; set; }

        public List<ProductBrandLookupTableDto> ProductBrandList { get; set; }

        public string ImageFileName { get; set; }
        public string ImageFileAcceptedTypes { get; set; }

        public bool IsEditMode => Product.Id.HasValue;
    }
}