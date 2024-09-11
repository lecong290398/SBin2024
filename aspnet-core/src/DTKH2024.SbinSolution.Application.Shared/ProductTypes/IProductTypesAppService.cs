using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.ProductTypes.Dtos;
using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.ProductTypes
{
    public interface IProductTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductTypeForViewDto>> GetAll(GetAllProductTypesInput input);

        Task<GetProductTypeForViewDto> GetProductTypeForView(int id);

        Task<GetProductTypeForEditOutput> GetProductTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditProductTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetProductTypesToExcel(GetAllProductTypesForExcelInput input);

    }
}