using DTKH2024.SbinSolution.Products.Dtos;

namespace DTKH2024.SbinSolution.ProductPromotions.Dtos
{
    public class GetProductPromotionForViewDto
    {
        public ProductPromotionDto ProductPromotion { get; set; }

        public string ProductProductName { get; set; }

        public string CategoryPromotionName { get; set; }
        public string BrandName { get; set; }
    }

    public class GetProductPromotionForCustomerDto : GetProductPromotionForViewDto
    {
        public ProductDtoForCustomer  InformationProduct { get; set; }

    }


}