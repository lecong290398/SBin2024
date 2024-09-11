using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using DTKH2024.SbinSolution.DataExporting.Excel.MiniExcel;
using DTKH2024.SbinSolution.WareHouseGifts.Dtos;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.WareHouseGifts.Exporting
{
    public class WareHouseGiftsExcelExporter : MiniExcelExcelExporterBase, IWareHouseGiftsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public WareHouseGiftsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetWareHouseGiftForViewDto> wareHouseGifts)
        {

            var items = new List<Dictionary<string, object>>();

            foreach (var wareHouseGift in wareHouseGifts)
            {
                items.Add(new Dictionary<string, object>()
                    {
                        {L("Code"), wareHouseGift.WareHouseGift.Code},
                        {L("IsUsed"), wareHouseGift.WareHouseGift.IsUsed},

                    });
            }

            return CreateExcelPackage("WareHouseGiftsList.xlsx", items);

        }
    }
}