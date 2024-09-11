using System.Collections.Generic;
using DTKH2024.SbinSolution.Products.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Products.Exporting
{
    public interface IProductsExcelExporter
    {
        FileDto ExportToFile(List<GetProductForViewDto> products);
    }
}