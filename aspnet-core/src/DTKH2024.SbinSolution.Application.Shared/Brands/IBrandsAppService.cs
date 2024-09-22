using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Brands.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.Brands
{
    public interface IBrandsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBrandForViewDto>> GetAll(GetAllBrandsInput input);

        Task<GetBrandForViewDto> GetBrandForView(int id);

        Task<GetBrandForEditOutput> GetBrandForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditBrandDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetBrandsToExcel(GetAllBrandsForExcelInput input);

        Task RemoveLogoFile(EntityDto input);

        Task<PagedResultDto<GetBrandForViewDto>> GetAllForClient(GetAllBrandsInput input);


    }
}