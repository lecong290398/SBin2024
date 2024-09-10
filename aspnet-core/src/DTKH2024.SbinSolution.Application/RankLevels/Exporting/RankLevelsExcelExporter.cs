using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.RankLevels.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.RankLevels.Exporting
{
    public class RankLevelsExcelExporter : MiniExcelExcelExporterBase, IRankLevelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RankLevelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRankLevelForViewDto> rankLevels)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var rankLevel in rankLevels)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Name"), rankLevel.RankLevel.Name},
                        {L("Description"), rankLevel.RankLevel.Description},
                        {L("MinimumPositiveScore"), rankLevel.RankLevel.MinimumPositiveScore},
                        {L("Color"), rankLevel.RankLevel.Color},
                        {L("Logo"), rankLevel.RankLevel.Logo},

                    });
            }

            return CreateExcelPackage("RankLevelsList.xlsx", items);

        }
    }
}