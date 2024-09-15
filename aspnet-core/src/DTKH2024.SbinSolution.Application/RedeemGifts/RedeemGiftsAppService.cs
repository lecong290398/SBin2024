using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.Brands;
using DTKH2024.SbinSolution.Brands.Dtos;
using System;
using System.Collections.Generic;
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

namespace DTKH2024.SbinSolution.RedeemGifts
{
    [AbpAuthorize(AppPermissions.Pages_RedeemGifts)]

    public class RedeemGiftsAppService : IRedeemGiftsAppService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public RedeemGiftsAppService(IRepository<Brand> brandRepository, IBinaryObjectManager binaryObjectManager)
        {
            _brandRepository = brandRepository;
            _binaryObjectManager = binaryObjectManager;
        }

        public async Task<PagedResultDto<GetBrandForViewDto>> GetAllBrand(GetAllBrandsInput input)
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

        protected virtual async Task<string> GetBinaryFileName(Guid? fileId)
        {
            if (!fileId.HasValue)
            {
                return null;
            }

            var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value);
            return file?.Description;
        }

    }
}
