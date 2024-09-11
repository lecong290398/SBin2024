using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.CategoryPromotions.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.CategoryPromotions.Exporting
{
    public class CategoryPromotionsExcelExporter : MiniExcelExcelExporterBase, ICategoryPromotionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CategoryPromotionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCategoryPromotionForViewDto> categoryPromotions)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var categoryPromotion in categoryPromotions)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Name"), categoryPromotion.CategoryPromotion.Name},
                        {L("Description"), categoryPromotion.CategoryPromotion.Description},
                        {L("Color"), categoryPromotion.CategoryPromotion.Color},

                    });
            }

            return CreateExcelPackage("CategoryPromotionsList.xlsx", items);

        }
    }
}