using System.Collections.Generic;
using DTKH2024.SbinSolution.RankLevels.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.RankLevels.Exporting
{
    public interface IRankLevelsExcelExporter
    {
        FileDto ExportToFile(List<GetRankLevelForViewDto> rankLevels);
    }
}