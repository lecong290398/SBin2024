using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.OrderHistories;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.OrderHistories;
using DTKH2024.SbinSolution.OrderHistories.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_OrderHistories)]
    public class OrderHistoriesController : SbinSolutionControllerBase
    {
        private readonly IOrderHistoriesAppService _orderHistoriesAppService;

        public OrderHistoriesController(IOrderHistoriesAppService orderHistoriesAppService)
        {
            _orderHistoriesAppService = orderHistoriesAppService;

        }

        public ActionResult Index()
        {
            var model = new OrderHistoriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_OrderHistories_Create, AppPermissions.Pages_OrderHistories_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetOrderHistoryForEditOutput getOrderHistoryForEditOutput;

            if (id.HasValue)
            {
                getOrderHistoryForEditOutput = await _orderHistoriesAppService.GetOrderHistoryForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getOrderHistoryForEditOutput = new GetOrderHistoryForEditOutput
                {
                    OrderHistory = new CreateOrEditOrderHistoryDto()
                };
            }

            var viewModel = new CreateOrEditOrderHistoryModalViewModel()
            {
                OrderHistory = getOrderHistoryForEditOutput.OrderHistory,
                UserName = getOrderHistoryForEditOutput.UserName,
                TransactionBinTransactionCode = getOrderHistoryForEditOutput.TransactionBinTransactionCode,
                WareHouseGiftCode = getOrderHistoryForEditOutput.WareHouseGiftCode,
                HistoryTypeName = getOrderHistoryForEditOutput.HistoryTypeName,
                OrderHistoryHistoryTypeList = await _orderHistoriesAppService.GetAllHistoryTypeForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewOrderHistoryModal(int id)
        {
            var getOrderHistoryForViewDto = await _orderHistoriesAppService.GetOrderHistoryForView(id);

            var model = new OrderHistoryViewModel()
            {
                OrderHistory = getOrderHistoryForViewDto.OrderHistory
                ,
                UserName = getOrderHistoryForViewDto.UserName

                ,
                TransactionBinTransactionCode = getOrderHistoryForViewDto.TransactionBinTransactionCode

                ,
                WareHouseGiftCode = getOrderHistoryForViewDto.WareHouseGiftCode

                ,
                HistoryTypeName = getOrderHistoryForViewDto.HistoryTypeName

            };

            return PartialView("_ViewOrderHistoryModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_OrderHistories_Create, AppPermissions.Pages_OrderHistories_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new OrderHistoryUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_OrderHistoryUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_OrderHistories_Create, AppPermissions.Pages_OrderHistories_Edit)]
        public PartialViewResult TransactionBinLookupTableModal(int? id, string displayName)
        {
            var viewModel = new OrderHistoryTransactionBinLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_OrderHistoryTransactionBinLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_OrderHistories_Create, AppPermissions.Pages_OrderHistories_Edit)]
        public PartialViewResult WareHouseGiftLookupTableModal(int? id, string displayName)
        {
            var viewModel = new OrderHistoryWareHouseGiftLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_OrderHistoryWareHouseGiftLookupTableModal", viewModel);
        }

    }
}