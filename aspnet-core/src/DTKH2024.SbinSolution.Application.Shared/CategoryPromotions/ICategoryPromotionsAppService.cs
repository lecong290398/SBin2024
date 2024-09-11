using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.CategoryPromotions.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.CategoryPromotions
{
    public interface ICategoryPromotionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCategoryPromotionForViewDto>> GetAll(GetAllCategoryPromotionsInput input);

        Task<GetCategoryPromotionForViewDto> GetCategoryPromotionForView(int id);

        Task<GetCategoryPromotionForEditOutput> GetCategoryPromotionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCategoryPromotionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCategoryPromotionsToExcel(GetAllCategoryPromotionsForExcelInput input);

    }
}