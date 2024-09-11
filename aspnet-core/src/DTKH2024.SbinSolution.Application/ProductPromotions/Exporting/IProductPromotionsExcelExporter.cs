using System.Collections.Generic;
using DTKH2024.SbinSolution.ProductPromotions.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.ProductPromotions.Exporting
{
    public interface IProductPromotionsExcelExporter
    {
        FileDto ExportToFile(List<GetProductPromotionForViewDto> productPromotions);
    }
}