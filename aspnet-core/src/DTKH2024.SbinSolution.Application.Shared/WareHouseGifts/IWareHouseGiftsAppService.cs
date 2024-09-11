using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.WareHouseGifts.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.WareHouseGifts
{
    public interface IWareHouseGiftsAppService : IApplicationService
    {
        Task<PagedResultDto<GetWareHouseGiftForViewDto>> GetAll(GetAllWareHouseGiftsInput input);

        Task<GetWareHouseGiftForViewDto> GetWareHouseGiftForView(int id);

        Task<GetWareHouseGiftForEditOutput> GetWareHouseGiftForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditWareHouseGiftDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetWareHouseGiftsToExcel(GetAllWareHouseGiftsForExcelInput input);

        Task<PagedResultDto<WareHouseGiftUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<WareHouseGiftProductPromotionLookupTableDto>> GetAllProductPromotionForLookupTable(GetAllForLookupTableInput input);

    }
}