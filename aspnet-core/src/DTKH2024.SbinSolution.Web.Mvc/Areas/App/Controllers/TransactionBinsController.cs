using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.TransactionBins;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.TransactionBins;
using DTKH2024.SbinSolution.TransactionBins.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_TransactionBins)]
    public class TransactionBinsController : SbinSolutionControllerBase
    {
        private readonly ITransactionBinsAppService _transactionBinsAppService;

        public TransactionBinsController(ITransactionBinsAppService transactionBinsAppService)
        {
            _transactionBinsAppService = transactionBinsAppService;

        }

        public ActionResult Index()
        {
            var model = new TransactionBinsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_TransactionBins_Create, AppPermissions.Pages_Administration_TransactionBins_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetTransactionBinForEditOutput getTransactionBinForEditOutput;

            if (id.HasValue)
            {
                getTransactionBinForEditOutput = await _transactionBinsAppService.GetTransactionBinForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getTransactionBinForEditOutput = new GetTransactionBinForEditOutput
                {
                    TransactionBin = new CreateOrEditTransactionBinDto()
                };
            }

            var viewModel = new CreateOrEditTransactionBinModalViewModel()
            {
                TransactionBin = getTransactionBinForEditOutput.TransactionBin,
                DeviceName = getTransactionBinForEditOutput.DeviceName,
                UserName = getTransactionBinForEditOutput.UserName,
                TransactionStatusName = getTransactionBinForEditOutput.TransactionStatusName,
                TransactionBinTransactionStatusList = await _transactionBinsAppService.GetAllTransactionStatusForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewTransactionBinModal(int id)
        {
            var getTransactionBinForViewDto = await _transactionBinsAppService.GetTransactionBinForView(id);

            var model = new TransactionBinViewModel()
            {
                TransactionBin = getTransactionBinForViewDto.TransactionBin
                ,
                DeviceName = getTransactionBinForViewDto.DeviceName

                ,
                UserName = getTransactionBinForViewDto.UserName

                ,
                TransactionStatusName = getTransactionBinForViewDto.TransactionStatusName

            };

            return PartialView("_ViewTransactionBinModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_TransactionBins_Create, AppPermissions.Pages_Administration_TransactionBins_Edit)]
        public PartialViewResult DeviceLookupTableModal(int? id, string displayName)
        {
            var viewModel = new TransactionBinDeviceLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TransactionBinDeviceLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_Administration_TransactionBins_Create, AppPermissions.Pages_Administration_TransactionBins_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new TransactionBinUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TransactionBinUserLookupTableModal", viewModel);
        }

    }
}