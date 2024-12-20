﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Brands.Dtos;
using DTKH2024.SbinSolution.ProductPromotions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DTKH2024.SbinSolution.RedeemGifts
{
    public interface IRedeemGiftsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBrandForViewDto>> GetAllBrand(GetAllBrandsInput input);
        Task<PagedResultDto<GetProductPromotionForViewDto>> GetAllProduct(GetAllProductPromotionsInputForCustomer input);
        Task<GetProductPromotionForCustomerDto> GetProductPromotionDetail(GetProductPromotionsInputForCustomer input);

        Task<int> CreateRedeemGift(int productPromotionId);
    }
}
