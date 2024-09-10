using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.Brands.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.Brands.Exporting
{
    public class BrandsExcelExporter : MiniExcelExcelExporterBase, IBrandsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BrandsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBrandForViewDto> brands)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var brand in brands)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Name"), brand.Brand.Name},
                        {L("Description"), brand.Brand.Description},
                        {L("Logo"), brand.Brand.Logo},

                    });
            }

            return CreateExcelPackage("BrandsList.xlsx", items);

        }
    }
}