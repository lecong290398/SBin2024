using System.Collections.Generic;
using DTKH2024.SbinSolution.ProductTypes.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.ProductTypes.Exporting
{
    public interface IProductTypesExcelExporter
    {
        FileDto ExportToFile(List<GetProductTypeForViewDto> productTypes);
    }
}