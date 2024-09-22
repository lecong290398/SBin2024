using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.Brands.Exporting;
using DTKH2024.SbinSolution.Brands.Dtos;
using DTKH2024.SbinSolution.Dto;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.Brands
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Brands)]
    public class BrandsAppService : SbinSolutionAppServiceBase, IBrandsAppService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IBrandsExcelExporter _brandsExcelExporter;

        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public BrandsAppService(IRepository<Brand> brandRepository, IBrandsExcelExporter brandsExcelExporter, ITempFileCacheManager tempFileCacheManager, IBinaryObjectManager binaryObjectManager)
        {
            _brandRepository = brandRepository;
            _brandsExcelExporter = brandsExcelExporter;

            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;

        }

        public virtual async Task<PagedResultDto<GetBrandForViewDto>> GetAll(GetAllBrandsInput input)
        {

            var filteredBrands = _brandRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredBrands = filteredBrands
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var brands = from o in pagedAndFilteredBrands
                         select new
                         {

                             o.Name,
                             o.Description,
                             o.Logo,
                             Id = o.Id
                         };

            var totalCount = await filteredBrands.CountAsync();

            var dbList = await brands.ToListAsync();
            var results = new List<GetBrandForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBrandForViewDto()
                {
                    Brand = new BrandDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Logo = o.Logo,
                        Id = o.Id,
                    }
                };
                res.Brand.LogoFileName = await GetBinaryFileName(o.Logo);

                results.Add(res);
            }

            return new PagedResultDto<GetBrandForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetBrandForViewDto> GetBrandForView(int id)
        {
            var brand = await _brandRepository.GetAsync(id);

            var output = new GetBrandForViewDto { Brand = ObjectMapper.Map<BrandDto>(brand) };

            output.Brand.LogoFileName = await GetBinaryFileName(brand.Logo);

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Brands_Edit)]
        public virtual async Task<GetBrandForEditOutput> GetBrandForEdit(EntityDto input)
        {
            var brand = await _brandRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBrandForEditOutput { Brand = ObjectMapper.Map<CreateOrEditBrandDto>(brand) };

            output.LogoFileName = await GetBinaryFileName(brand.Logo);

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditBrandDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Brands_Create)]
        protected virtual async Task Create(CreateOrEditBrandDto input)
        {
            var brand = ObjectMapper.Map<Brand>(input);

            await _brandRepository.InsertAsync(brand);
            brand.Logo = await GetBinaryObjectFromCache(input.LogoToken);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Brands_Edit)]
        protected virtual async Task Update(CreateOrEditBrandDto input)
        {
            var brand = await _brandRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, brand);
            brand.Logo = await GetBinaryObjectFromCache(input.LogoToken);

        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Brands_Delete)]
        public virtual async Task Delete(EntityDto input)
        {
            await _brandRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetBrandsToExcel(GetAllBrandsForExcelInput input)
        {

            var filteredBrands = _brandRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredBrands
                         select new GetBrandForViewDto()
                         {
                             Brand = new BrandDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Logo = o.Logo,
                                 Id = o.Id
                             }
                         });

            var brandListDtos = await query.ToListAsync();

            return _brandsExcelExporter.ExportToFile(brandListDtos);
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

        [AbpAuthorize(AppPermissions.Pages_Administration_Brands_Edit)]
        public virtual async Task RemoveLogoFile(EntityDto input)
        {
            var brand = await _brandRepository.FirstOrDefaultAsync(input.Id);
            if (brand == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!brand.Logo.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _binaryObjectManager.DeleteAsync(brand.Logo.Value);
            brand.Logo = null;
        }

        [AbpAllowAnonymous]
        public virtual async Task<PagedResultDto<GetBrandForViewDto>> GetAllForClient(GetAllBrandsInput input)
        {
            var filteredBrands = _brandRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredBrands = filteredBrands
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var brands = from o in pagedAndFilteredBrands
                         select new
                         {

                             o.Name,
                             o.Description,
                             o.Logo,
                             Id = o.Id
                         };

            var totalCount = await filteredBrands.CountAsync();

            var dbList = await brands.ToListAsync();
            var results = new List<GetBrandForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBrandForViewDto()
                {
                    Brand = new BrandDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Logo = o.Logo,
                        Id = o.Id,
                    }
                };
                res.Brand.LogoFileName = await GetBinaryFileName(o.Logo);

                results.Add(res);
            }

            return new PagedResultDto<GetBrandForViewDto>(
                totalCount,
                results
            );
        }
    }
}