using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.BenefitsRankLevels.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.BenefitsRankLevels.Exporting
{
    public class BenefitsRankLevelsExcelExporter : MiniExcelExcelExporterBase, IBenefitsRankLevelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BenefitsRankLevelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBenefitsRankLevelForViewDto> benefitsRankLevels)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var benefitsRankLevel in benefitsRankLevels)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Name"), benefitsRankLevel.BenefitsRankLevel.Name},
                        {L("Description"), benefitsRankLevel.BenefitsRankLevel.Description},

                    });
            }

            return CreateExcelPackage("BenefitsRankLevelsList.xlsx", items);

        }
    }
}