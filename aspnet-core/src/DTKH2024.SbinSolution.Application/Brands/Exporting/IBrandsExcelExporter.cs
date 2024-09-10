using System.Collections.Generic;
using DTKH2024.SbinSolution.Brands.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Brands.Exporting
{
    public interface IBrandsExcelExporter
    {
        FileDto ExportToFile(List<GetBrandForViewDto> brands);
    }
}