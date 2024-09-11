using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.Products.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.Products.Exporting
{
    public class ProductsExcelExporter : MiniExcelExcelExporterBase, IProductsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProductsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProductForViewDto> products)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var product in products)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("ProductName"), product.Product.ProductName},
                        {L("TimeDescription"), product.Product.TimeDescription},
                        {L("ApplicableSubjects"), product.Product.ApplicableSubjects},
                        {L("Regulations"), product.Product.Regulations},
                        {L("UserManual"), product.Product.UserManual},
                        {L("ScopeOfApplication"), product.Product.ScopeOfApplication},
                        {L("SupportAndComplaints"), product.Product.SupportAndComplaints},
                        {L("Description"), product.Product.Description},
                        {L("Image"), product.Product.Image},

                    });
            }

            return CreateExcelPackage("ProductsList.xlsx", items);

        }
    }
}