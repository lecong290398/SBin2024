using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.ProductTypes.Exporting;
using DTKH2024.SbinSolution.ProductTypes.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.ProductTypes
{
    [AbpAuthorize(AppPermissions.Pages_Administration_ProductTypes)]
    public class ProductTypesAppService : SbinSolutionAppServiceBase, IProductTypesAppService
    {
        private readonly IRepository<ProductType> _productTypeRepository;
        private readonly IProductTypesExcelExporter _productTypesExcelExporter;

        public ProductTypesAppService(IRepository<ProductType> productTypeRepository, IProductTypesExcelExporter productTypesExcelExporter)
        {
            _productTypeRepository = productTypeRepository;
            _productTypesExcelExporter = productTypesExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetProductTypeForViewDto>> GetAll(GetAllProductTypesInput input)
        {

            var filteredProductTypes = _productTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredProductTypes = filteredProductTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productTypes = from o in pagedAndFilteredProductTypes
                               select new
                               {

                                   o.Name,
                                   o.Description,
                                   o.Color,
                                   Id = o.Id
                               };

            var totalCount = await filteredProductTypes.CountAsync();

            var dbList = await productTypes.ToListAsync();
            var results = new List<GetProductTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductTypeForViewDto()
                {
                    ProductType = new ProductTypeDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Color = o.Color,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductTypeForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetProductTypeForViewDto> GetProductTypeForView(int id)
        {
            var productType = await _productTypeRepository.GetAsync(id);

            var output = new GetProductTypeForViewDto { ProductType = ObjectMapper.Map<ProductTypeDto>(productType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ProductTypes_Edit)]
        public virtual async Task<GetProductTypeForEditOutput> GetProductTypeForEdit(EntityDto input)
        {
            var productType = await _productTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductTypeForEditOutput { ProductType = ObjectMapper.Map<CreateOrEditProductTypeDto>(productType) };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditProductTypeDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ProductTypes_Create)]
        protected virtual async Task Create(CreateOrEditProductTypeDto input)
        {
            var productType = ObjectMapper.Map<ProductType>(input);

            await _productTypeRepository.InsertAsync(productType);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ProductTypes_Edit)]
        protected virtual async Task Update(CreateOrEditProductTypeDto input)
        {
            var productType = await _productTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, productType);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ProductTypes_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _productTypeRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetProductTypesToExcel(GetAllProductTypesForExcelInput input)
        {

            var filteredProductTypes = _productTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.Color.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredProductTypes
                         select new GetProductTypeForViewDto()
                         {
                             ProductType = new ProductTypeDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Color = o.Color,
                                 Id = o.Id
                             }
                         });

            var productTypeListDtos = await query.ToListAsync();

            return _productTypesExcelExporter.ExportToFile(productTypeListDtos);
        }

    }
}