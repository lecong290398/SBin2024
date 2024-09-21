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
using DTKH2024.SbinSolution.Products;
using DTKH2024.SbinSolution.ProductPromotions.Dtos;
using DTKH2024.SbinSolution.ProductPromotions;
using DTKH2024.SbinSolution.CategoryPromotions;
using DTKH2024.SbinSolution.Products.Dtos;
using AutoMapper.Internal.Mappers;
using DTKH2024.SbinSolution.OrderHistories.Dtos;
using DTKH2024.SbinSolution.OrderHistories;
using Abp.Runtime.Session;
using DTKH2024.SbinSolution.WareHouseGifts;
using DTKH2024.SbinSolution.WareHouseGifts.Dtos;
using DTKH2024.SbinSolution.HistoryTypes;
using Abp;

namespace DTKH2024.SbinSolution.RedeemGifts
{
    [AbpAuthorize(AppPermissions.Pages_RedeemGifts)]

    public class RedeemGiftsAppService : SbinSolutionAppServiceBase, IRedeemGiftsAppService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductPromotion> _productPromotionRepository;
        private readonly IRepository<Product, int> _lookup_productRepository;
        private readonly IRepository<CategoryPromotion, int> _lookup_categoryPromotionRepository;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<OrderHistory> _orderHistoryRepository;
        private readonly IRepository<WareHouseGift> _wareHouseGiftRepository;
        private readonly IRepository<HistoryType> _historyTypeRepository;

        public RedeemGiftsAppService(IRepository<Brand> brandRepository, IRepository<ProductPromotion> productPromotionRepository
            , IRepository<Product> productRepository, IRepository<Product, int> lookup_productRepository, IRepository<CategoryPromotion, int> lookup_categoryPromotionRepository
            , IBinaryObjectManager binaryObjectManager, IAbpSession abpSession, IRepository<OrderHistory> orderHistoryRepository, IRepository<WareHouseGift> wareHouseGiftRepository, IRepository<HistoryType> historyTypeRepository)
        {
            _brandRepository = brandRepository;
            _binaryObjectManager = binaryObjectManager;
            _productRepository = productRepository;
            _productPromotionRepository = productPromotionRepository;
            _lookup_productRepository = lookup_productRepository;
            _lookup_categoryPromotionRepository = lookup_categoryPromotionRepository;
            _abpSession = abpSession;
            _orderHistoryRepository = orderHistoryRepository;
            _wareHouseGiftRepository = wareHouseGiftRepository;
            _historyTypeRepository = historyTypeRepository;
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
                var countProduct = await _productRepository.CountAsync(c => c.BrandId == o.Id);
                var res = new GetBrandForViewDto()
                {
                    Brand = new BrandDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        Logo = o.Logo,
                        Id = o.Id,
                        ProductCount = countProduct
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

        public virtual async Task<PagedResultDto<GetProductPromotionForViewDto>> GetAllProduct(GetAllProductPromotionsInputForCustomer input)
        {

            var filteredProductPromotions = _productPromotionRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductFk.BrandFk)
                        .Include(e => e.CategoryPromotionFk)
                        .Where(e => e.QuantityCurrent < e.QuantityInStock && e.EndDate >= DateTime.Now)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PromotionCode.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(input.MinPointFilter != null, e => e.Point >= input.MinPointFilter)
                        .WhereIf(input.MaxPointFilter != null, e => e.Point <= input.MaxPointFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.BrandID != null, e => e.ProductFk.BrandId == input.BrandID)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PromotionCodeFilter), e => e.PromotionCode.Contains(input.PromotionCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductProductNameFilter), e => e.ProductFk != null && e.ProductFk.ProductName == input.ProductProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CategoryPromotionNameFilter), e => e.CategoryPromotionFk != null && e.CategoryPromotionFk.Name == input.CategoryPromotionNameFilter);

            var pagedAndFilteredProductPromotions = filteredProductPromotions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productPromotions = from o in pagedAndFilteredProductPromotions
                                    join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_categoryPromotionRepository.GetAll() on o.CategoryPromotionId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()

                                    join o3 in _brandRepository.GetAll() on s1.BrandId equals o3.Id into j3
                                    from s3 in j3.DefaultIfEmpty()

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
                                        ProductProductName = s1 == null || s1.ProductName == null ? "" : s1.ProductName.ToString(),
                                        CategoryPromotionName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                        BrandName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                                        o.ProductId
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
                        ProductId = o.ProductId
                    },
                    ProductProductName = o.ProductProductName,
                    CategoryPromotionName = o.CategoryPromotionName,
                    BrandName = o.BrandName,

                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductPromotionForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetProductPromotionForCustomerDto> GetProductPromotionDetail(GetProductPromotionsInputForCustomer input)
        {
            var filteredProductPromotions = await _productPromotionRepository.GetAll()
                .Include(e => e.ProductFk)
                .Include(e => e.ProductFk.BrandFk)
                .Include(e => e.CategoryPromotionFk)
                .Where(c => c.ProductId == input.ProductID && c.Id == input.ProductPromotionID)
                .Select(o => new
                {
                    o.Point,
                    o.QuantityCurrent,
                    o.QuantityInStock,
                    o.StartDate,
                    o.EndDate,
                    o.PromotionCode,
                    o.Description,
                    Id = o.Id,
                    ProductProductName = o.ProductFk.ProductName,
                    CategoryPromotionName = o.CategoryPromotionFk.Name,
                    BrandName = o.ProductFk.BrandFk.Name,
                    ProductFk = o.ProductFk
                })
                .FirstOrDefaultAsync();

            if (filteredProductPromotions == null)
            {
                throw new UserFriendlyException("Product promotion not found");
            }

            var result = new GetProductPromotionForCustomerDto()
            {
                ProductPromotion = new ProductPromotionDto
                {
                    Point = filteredProductPromotions.Point,
                    QuantityCurrent = filteredProductPromotions.QuantityCurrent,
                    QuantityInStock = filteredProductPromotions.QuantityInStock,
                    StartDate = filteredProductPromotions.StartDate,
                    EndDate = filteredProductPromotions.EndDate,
                    PromotionCode = filteredProductPromotions.PromotionCode,
                    Description = filteredProductPromotions.Description,
                    Id = filteredProductPromotions.Id,
                },
                ProductProductName = filteredProductPromotions.ProductProductName,
                CategoryPromotionName = filteredProductPromotions.CategoryPromotionName,
                InformationProduct = new ProductDtoForCustomer
                {
                    ProductName = filteredProductPromotions.ProductProductName,
                    TimeDescription = filteredProductPromotions.ProductFk.TimeDescription,
                    ApplicableSubjects = filteredProductPromotions.ProductFk.ApplicableSubjects,
                    Regulations = filteredProductPromotions.ProductFk.Regulations,
                    UserManual = filteredProductPromotions.ProductFk.UserManual,
                    ScopeOfApplication = filteredProductPromotions.ProductFk.ScopeOfApplication,
                    SupportAndComplaints = filteredProductPromotions.ProductFk.SupportAndComplaints,
                    Description = filteredProductPromotions.ProductFk.Description,
                    Id = filteredProductPromotions.Id,
                    BrandId = filteredProductPromotions.ProductFk.BrandId,
                    BrandName = filteredProductPromotions.BrandName
                }
            };

            return result;
        }

        public virtual async Task CreateRedeemGift(int productPromotionId)
        {
            try
            {
                var userId = _abpSession.UserId ?? throw new UserFriendlyException("You are not logged in to the system.");
                var userCurrent = await UserManager.FindByIdAsync(userId.ToString());
                if (userCurrent is null)
                {
                    throw new UserFriendlyException(L("UserNotFound"));
                }

                var productPromotion = await _productPromotionRepository.FirstOrDefaultAsync(productPromotionId)
                    ?? throw new UserFriendlyException("Product promotion not found.");

                if (productPromotion.QuantityCurrent >= productPromotion.QuantityInStock)
                {
                    throw new UserFriendlyException("Gift out of stock");
                }
                if (productPromotion.EndDate < DateTime.Now)
                {
                    throw new UserFriendlyException("Gift expired");
                }

                var historyType = await _historyTypeRepository.FirstOrDefaultAsync(AppConsts.HistoryType_DoiQua)
                    ?? throw new UserFriendlyException("An error occurred, please try again later.");

                var wareHouseGift = new WareHouseGift
                {
                    ProductPromotionId = productPromotion.Id,
                    UserId = userId,
                    Code = AppConsts.getCodeRandom(AppConsts.keyPerfixWareHouseGift)
                };
                var wareHouseGiftId = await _wareHouseGiftRepository.InsertAndGetIdAsync(wareHouseGift).ConfigureAwait(false);
                if (wareHouseGiftId != 0)
                {
                    var orderHistory = new OrderHistory
                    {
                        Description = historyType.Name,
                        Point = productPromotion.Point,
                        UserId = userId,
                        WareHouseGiftId = wareHouseGiftId,
                        HistoryTypeId = historyType.Id
                    };
                    await _orderHistoryRepository.InsertAsync(orderHistory);

                    productPromotion.QuantityCurrent += 1;
                    await _productPromotionRepository.UpdateAsync(productPromotion);

                    if (userCurrent.Point >= productPromotion.Point)
                    {
                        userCurrent.Point -= productPromotion.Point;
                        userCurrent.PositivePoint += productPromotion.Point;
                        await UserManager.UpdateAsync(userCurrent);
                    }else
                    {
                        throw new UserFriendlyException("Not enough points to redeem");
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
