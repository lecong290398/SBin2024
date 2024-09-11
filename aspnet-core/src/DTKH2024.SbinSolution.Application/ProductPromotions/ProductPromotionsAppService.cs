using DTKH2024.SbinSolution.Products;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.ProductPromotions.Exporting;
using DTKH2024.SbinSolution.ProductPromotions.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.ProductPromotions
{
    [AbpAuthorize(AppPermissions.Pages_Administration_ProductPromotions)]
    public class ProductPromotionsAppService : SbinSolutionAppServiceBase, IProductPromotionsAppService
    {
        private readonly IRepository<ProductPromotion> _productPromotionRepository;
        private readonly IProductPromotionsExcelExporter _productPromotionsExcelExporter;
        private readonly IRepository<Product, int> _lookup_productRepository;

        public ProductPromotionsAppService(IRepository<ProductPromotion> productPromotionRepository, IProductPromotionsExcelExporter productPromotionsExcelExporter, IRepository<Product, int> lookup_productRepository)
        {
            _productPromotionRepository = productPromotionRepository;
            _productPromotionsExcelExporter = productPromotionsExcelExporter;
            _lookup_productRepository = lookup_productRepository;

        }

        public virtual async Task<PagedResultDto<GetProductPromotionForViewDto>> GetAll(GetAllProductPromotionsInput input)
        {

            var filteredProductPromotions = _productPromotionRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PromotionCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinPointFilter != null, e => e.Point >= input.MinPointFilter)
                        .WhereIf(input.MaxPointFilter != null, e => e.Point <= input.MaxPointFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromotionCodeFilter), e => e.PromotionCode.Contains(input.PromotionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductProductNameFilter), e => e.ProductFk != null && e.ProductFk.ProductName == input.ProductProductNameFilter);

            var pagedAndFilteredProductPromotions = filteredProductPromotions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productPromotions = from o in pagedAndFilteredProductPromotions
                                    join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    select new
                                    {

                                        o.Point,
                                        o.QuantityCurrent,
                                        o.QuantityInStock,
                                        o.StartDate,
                                        o.EndDate,
                                        o.PromotionCode,
                                        o.Description,
                                        Id = o.Id,
                                        ProductProductName = s1 == null || s1.ProductName == null ? "" : s1.ProductName.ToString()
                                    };

            var totalCount = await filteredProductPromotions.CountAsync();

            var dbList = await productPromotions.ToListAsync();
            var results = new List<GetProductPromotionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductPromotionForViewDto()
                {
                    ProductPromotion = new ProductPromotionDto
                    {

                        Point = o.Point,
                        QuantityCurrent = o.QuantityCurrent,
                        QuantityInStock = o.QuantityInStock,
                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        PromotionCode = o.PromotionCode,
                        Description = o.Description,
                        Id = o.Id,
                    },
                    ProductProductName = o.ProductProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductPromotionForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetProductPromotionForViewDto> GetProductPromotionForView(int id)
        {
            var productPromotion = await _productPromotionRepository.GetAsync(id);

            var output = new GetProductPromotionForViewDto { ProductPromotion = ObjectMapper.Map<ProductPromotionDto>(productPromotion) };

            if (output.ProductPromotion.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((int)output.ProductPromotion.ProductId);
                output.ProductProductName = _lookupProduct?.ProductName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ProductPromotions_Edit)]
        public virtual async Task<GetProductPromotionForEditOutput> GetProductPromotionForEdit(EntityDto input)
        {
            var productPromotion = await _productPromotionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductPromotionForEditOutput { ProductPromotion = ObjectMapper.Map<CreateOrEditProductPromotionDto>(productPromotion) };

            if (output.ProductPromotion.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((int)output.ProductPromotion.ProductId);
                output.ProductProductName = _lookupProduct?.ProductName?.ToString();
            }

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditProductPromotionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_ProductPromotions_Create)]
        protected virtual async Task Create(CreateOrEditProductPromotionDto input)
        {
            var productPromotion = ObjectMapper.Map<ProductPromotion>(input);

            await _productPromotionRepository.InsertAsync(productPromotion);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ProductPromotions_Edit)]
        protected virtual async Task Update(CreateOrEditProductPromotionDto input)
        {
            var productPromotion = await _productPromotionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, productPromotion);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ProductPromotions_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _productPromotionRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetProductPromotionsToExcel(GetAllProductPromotionsForExcelInput input)
        {

            var filteredProductPromotions = _productPromotionRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PromotionCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinPointFilter != null, e => e.Point >= input.MinPointFilter)
                        .WhereIf(input.MaxPointFilter != null, e => e.Point <= input.MaxPointFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromotionCodeFilter), e => e.PromotionCode.Contains(input.PromotionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductProductNameFilter), e => e.ProductFk != null && e.ProductFk.ProductName == input.ProductProductNameFilter);

            var query = (from o in filteredProductPromotions
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetProductPromotionForViewDto()
                         {
                             ProductPromotion = new ProductPromotionDto
                             {
                                 Point = o.Point,
                                 QuantityCurrent = o.QuantityCurrent,
                                 QuantityInStock = o.QuantityInStock,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 PromotionCode = o.PromotionCode,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             ProductProductName = s1 == null || s1.ProductName == null ? "" : s1.ProductName.ToString()
                         });

            var productPromotionListDtos = await query.ToListAsync();

            return _productPromotionsExcelExporter.ExportToFile(productPromotionListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_ProductPromotions)]
        public async Task<PagedResultDto<ProductPromotionProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ProductName != null && e.ProductName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductPromotionProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductPromotionProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.ProductName?.ToString()
                });
            }

            return new PagedResultDto<ProductPromotionProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}