using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.ProductPromotions.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.ProductPromotions
{
    public interface IProductPromotionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductPromotionForViewDto>> GetAll(GetAllProductPromotionsInput input);

        Task<GetProductPromotionForViewDto> GetProductPromotionForView(int id);

        Task<GetProductPromotionForEditOutput> GetProductPromotionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditProductPromotionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetProductPromotionsToExcel(GetAllProductPromotionsForExcelInput input);

        Task<PagedResultDto<ProductPromotionProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input);

    }
}