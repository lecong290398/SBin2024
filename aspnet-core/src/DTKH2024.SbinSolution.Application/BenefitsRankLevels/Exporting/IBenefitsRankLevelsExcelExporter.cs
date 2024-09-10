using System.Collections.Generic;
using DTKH2024.SbinSolution.BenefitsRankLevels.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.BenefitsRankLevels.Exporting
{
    public interface IBenefitsRankLevelsExcelExporter
    {
        FileDto ExportToFile(List<GetBenefitsRankLevelForViewDto> benefitsRankLevels);
    }
}