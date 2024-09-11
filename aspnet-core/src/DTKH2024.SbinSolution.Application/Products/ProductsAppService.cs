using DTKH2024.SbinSolution.ProductTypes;
using DTKH2024.SbinSolution.Brands;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.Products.Exporting;
using DTKH2024.SbinSolution.Products.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.Products
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Products)]
    public class ProductsAppService : SbinSolutionAppServiceBase, IProductsAppService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IProductsExcelExporter _productsExcelExporter;
        private readonly IRepository<ProductType, int> _lookup_productTypeRepository;
        private readonly IRepository<Brand, int> _lookup_brandRepository;

        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public ProductsAppService(IRepository<Product> productRepository, IProductsExcelExporter productsExcelExporter, IRepository<ProductType, int> lookup_productTypeRepository, IRepository<Brand, int> lookup_brandRepository, ITempFileCacheManager tempFileCacheManager, IBinaryObjectManager binaryObjectManager)
        {
            _productRepository = productRepository;
            _productsExcelExporter = productsExcelExporter;
            _lookup_productTypeRepository = lookup_productTypeRepository;
            _lookup_brandRepository = lookup_brandRepository;

            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;

        }

        public virtual async Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input)
        {

            var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.ProductTypeFk)
                        .Include(e => e.BrandFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProductName.Contains(input.Filter) || e.TimeDescription.Contains(input.Filter) || e.ApplicableSubjects.Contains(input.Filter) || e.Regulations.Contains(input.Filter) || e.UserManual.Contains(input.Filter) || e.ScopeOfApplication.Contains(input.Filter) || e.SupportAndComplaints.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductName.Contains(input.ProductNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductTypeNameFilter), e => e.ProductTypeFk != null && e.ProductTypeFk.Name == input.ProductTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandNameFilter), e => e.BrandFk != null && e.BrandFk.Name == input.BrandNameFilter);

            var pagedAndFilteredProducts = filteredProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var products = from o in pagedAndFilteredProducts
                           join o1 in _lookup_productTypeRepository.GetAll() on o.ProductTypeId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_brandRepository.GetAll() on o.BrandId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           select new
                           {

                               o.ProductName,
                               o.TimeDescription,
                               o.ApplicableSubjects,
                               o.Regulations,
                               o.UserManual,
                               o.ScopeOfApplication,
                               o.SupportAndComplaints,
                               o.Description,
                               o.Image,
                               Id = o.Id,
                               ProductTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                               BrandName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                           };

            var totalCount = await filteredProducts.CountAsync();

            var dbList = await products.ToListAsync();
            var results = new List<GetProductForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductForViewDto()
                {
                    Product = new ProductDto
                    {

                        ProductName = o.ProductName,
                        TimeDescription = o.TimeDescription,
                        ApplicableSubjects = o.ApplicableSubjects,
                        Regulations = o.Regulations,
                        UserManual = o.UserManual,
                        ScopeOfApplication = o.ScopeOfApplication,
                        SupportAndComplaints = o.SupportAndComplaints,
                        Description = o.Description,
                        Image = o.Image,
                        Id = o.Id,
                    },
                    ProductTypeName = o.ProductTypeName,
                    BrandName = o.BrandName
                };
                res.Product.ImageFileName = await GetBinaryFileName(o.Image);

                results.Add(res);
            }

            return new PagedResultDto<GetProductForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetProductForViewDto> GetProductForView(int id)
        {
            var product = await _productRepository.GetAsync(id);

            var output = new GetProductForViewDto { Product = ObjectMapper.Map<ProductDto>(product) };

            if (output.Product.ProductTypeId != null)
            {
                var _lookupProductType = await _lookup_productTypeRepository.FirstOrDefaultAsync((int)output.Product.ProductTypeId);
                output.ProductTypeName = _lookupProductType?.Name?.ToString();
            }

            if (output.Product.BrandId != null)
            {
                var _lookupBrand = await _lookup_brandRepository.FirstOrDefaultAsync((int)output.Product.BrandId);
                output.BrandName = _lookupBrand?.Name?.ToString();
            }

            output.Product.ImageFileName = await GetBinaryFileName(product.Image);

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Products_Edit)]
        public virtual async Task<GetProductForEditOutput> GetProductForEdit(EntityDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductForEditOutput { Product = ObjectMapper.Map<CreateOrEditProductDto>(product) };

            if (output.Product.ProductTypeId != null)
            {
                var _lookupProductType = await _lookup_productTypeRepository.FirstOrDefaultAsync((int)output.Product.ProductTypeId);
                output.ProductTypeName = _lookupProductType?.Name?.ToString();
            }

            if (output.Product.BrandId != null)
            {
                var _lookupBrand = await _lookup_brandRepository.FirstOrDefaultAsync((int)output.Product.BrandId);
                output.BrandName = _lookupBrand?.Name?.ToString();
            }

            output.ImageFileName = await GetBinaryFileName(product.Image);

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditProductDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Products_Create)]
        protected virtual async Task Create(CreateOrEditProductDto input)
        {
            var product = ObjectMapper.Map<Product>(input);

            await _productRepository.InsertAsync(product);
            product.Image = await GetBinaryObjectFromCache(input.ImageToken);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Products_Edit)]
        protected virtual async Task Update(CreateOrEditProductDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, product);
            product.Image = await GetBinaryObjectFromCache(input.ImageToken);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Products_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _productRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input)
        {

            var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.ProductTypeFk)
                        .Include(e => e.BrandFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ProductName.Contains(input.Filter) || e.TimeDescription.Contains(input.Filter) || e.ApplicableSubjects.Contains(input.Filter) || e.Regulations.Contains(input.Filter) || e.UserManual.Contains(input.Filter) || e.ScopeOfApplication.Contains(input.Filter) || e.SupportAndComplaints.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductName.Contains(input.ProductNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductTypeNameFilter), e => e.ProductTypeFk != null && e.ProductTypeFk.Name == input.ProductTypeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandNameFilter), e => e.BrandFk != null && e.BrandFk.Name == input.BrandNameFilter);

            var query = (from o in filteredProducts
                         join o1 in _lookup_productTypeRepository.GetAll() on o.ProductTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_brandRepository.GetAll() on o.BrandId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductForViewDto()
                         {
                             Product = new ProductDto
                             {
                                 ProductName = o.ProductName,
                                 TimeDescription = o.TimeDescription,
                                 ApplicableSubjects = o.ApplicableSubjects,
                                 Regulations = o.Regulations,
                                 UserManual = o.UserManual,
                                 ScopeOfApplication = o.ScopeOfApplication,
                                 SupportAndComplaints = o.SupportAndComplaints,
                                 Description = o.Description,
                                 Image = o.Image,
                                 Id = o.Id
                             },
                             ProductTypeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             BrandName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productListDtos = await query.ToListAsync();

            return _productsExcelExporter.ExportToFile(productListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Products)]
        public async Task<List<ProductProductTypeLookupTableDto>> GetAllProductTypeForTableDropdown()
        {
            return await _lookup_productTypeRepository.GetAll()
                .Select(productType => new ProductProductTypeLookupTableDto
                {
                    Id = productType.Id,
                    DisplayName = productType == null || productType.Name == null ? "" : productType.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Products)]
        public async Task<List<ProductBrandLookupTableDto>> GetAllBrandForTableDropdown()
        {
            return await _lookup_brandRepository.GetAll()
                .Select(brand => new ProductBrandLookupTableDto
                {
                    Id = brand.Id,
                    DisplayName = brand == null || brand.Name == null ? "" : brand.Name.ToString()
                }).ToListAsync();
        }

        protected virtual async Task<Guid?> GetBinaryObjectFromCache(string fileToken)
        {
            if (fileToken.IsNullOrWhiteSpace())
            {
                return null;
            }

            var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

            if (fileCache == null)
            {
                throw new UserFriendlyException("There is no such file with the token: " + fileToken);
            }

            var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, fileCache.FileName);
            await _binaryObjectManager.SaveAsync(storedFile);

            return storedFile.Id;
        }

        protected virtual async Task<string> GetBinaryFileName(Guid? fileId)
        {
            if (!fileId.HasValue)
            {
                return null;
            }

            var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value);
            return file?.Description;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Products_Edit)]
        public virtual async Task RemoveImageFile(EntityDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync(input.Id);
            if (product == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!product.Image.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _binaryObjectManager.DeleteAsync(product.Image.Value);
            product.Image = null;
        }

    }
}