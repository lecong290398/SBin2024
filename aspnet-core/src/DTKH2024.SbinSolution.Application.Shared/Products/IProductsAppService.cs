using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Products.Dtos;
using DTKH2024.SbinSolution.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace DTKH2024.SbinSolution.Products
{
    public interface IProductsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input);

        Task<GetProductForViewDto> GetProductForView(int id);

        Task<GetProductForEditOutput> GetProductForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditProductDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input);

        Task<List<ProductProductTypeLookupTableDto>> GetAllProductTypeForTableDropdown();

        Task<List<ProductBrandLookupTableDto>> GetAllBrandForTableDropdown();

        Task RemoveImageFile(EntityDto input);

    }
}