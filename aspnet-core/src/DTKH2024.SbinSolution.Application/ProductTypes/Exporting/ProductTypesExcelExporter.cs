using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.ProductTypes.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.ProductTypes.Exporting
{
    public class ProductTypesExcelExporter : MiniExcelExcelExporterBase, IProductTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductTypeForViewDto> productTypes)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var productType in productTypes)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Name"), productType.ProductType.Name},
                        {L("Description"), productType.ProductType.Description},
                        {L("Color"), productType.ProductType.Color},

                    });
            }

            return CreateExcelPackage("ProductTypesList.xlsx", items);

        }
    }
}