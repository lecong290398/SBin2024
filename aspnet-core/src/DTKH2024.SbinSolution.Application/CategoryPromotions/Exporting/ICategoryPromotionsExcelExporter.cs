using System.Collections.Generic;
using DTKH2024.SbinSolution.CategoryPromotions.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.CategoryPromotions.Exporting
{
    public interface ICategoryPromotionsExcelExporter
    {
        FileDto ExportToFile(List<GetCategoryPromotionForViewDto> categoryPromotions);
    }
}