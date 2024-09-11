using System.Collections.Generic;
using DTKH2024.SbinSolution.WareHouseGifts.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.WareHouseGifts.Exporting
{
    public interface IWareHouseGiftsExcelExporter
    {
        FileDto ExportToFile(List<GetWareHouseGiftForViewDto> wareHouseGifts);
    }
}