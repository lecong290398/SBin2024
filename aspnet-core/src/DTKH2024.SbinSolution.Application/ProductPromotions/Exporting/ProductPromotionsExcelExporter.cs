using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.ProductPromotions.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.ProductPromotions.Exporting
{
    public class ProductPromotionsExcelExporter : MiniExcelExcelExporterBase, IProductPromotionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductPromotionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductPromotionForViewDto> productPromotions)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var productPromotion in productPromotions)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("PromotionName"), productPromotion.ProductPromotion.PromotionName},
                        {L("Point"), productPromotion.ProductPromotion.Point},
                        {L("QuantityCurrent"), productPromotion.ProductPromotion.QuantityCurrent},
                        {L("QuantityInStock"), productPromotion.ProductPromotion.QuantityInStock},
                        {L("StartDate"), productPromotion.ProductPromotion.StartDate},
                        {L("EndDate"), productPromotion.ProductPromotion.EndDate},

                    });
            }

            return CreateExcelPackage("ProductPromotionsList.xlsx", items);

        }
    }
}