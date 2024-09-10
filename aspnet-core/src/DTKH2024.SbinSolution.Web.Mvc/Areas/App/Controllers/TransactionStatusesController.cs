using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Areas.App.Models.TransactionStatuses;
using DTKH2024.SbinSolution.Web.Controllers;
using DTKH2024.SbinSolution.Authorization;
using DTKH2024.SbinSolution.TransactionStatuses;
using DTKH2024.SbinSolution.TransactionStatuses.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace DTKH2024.SbinSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_TransactionStatuses)]
    public class TransactionStatusesController : SbinSolutionControllerBase
    {
        private readonly ITransactionStatusesAppService _transactionStatusesAppService;

        public TransactionStatusesController(ITransactionStatusesAppService transactionStatusesAppService)
        {
            _transactionStatusesAppService = transactionStatusesAppService;

        }

        public ActionResult Index()
        {
            var model = new TransactionStatusesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_TransactionStatuses_Create, AppPermissions.Pages_Administration_TransactionStatuses_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetTransactionStatusForEditOutput getTransactionStatusForEditOutput;

            if (id.HasValue)
            {
                getTransactionStatusForEditOutput = await _transactionStatusesAppService.GetTransactionStatusForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getTransactionStatusForEditOutput = new GetTransactionStatusForEditOutput
                {
                    TransactionStatus = new CreateOrEditTransactionStatusDto()
                };
            }

            var viewModel = new CreateOrEditTransactionStatusModalViewModel()
            {
                TransactionStatus = getTransactionStatusForEditOutput.TransactionStatus,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewTransactionStatusModal(int id)
        {
            var getTransactionStatusForViewDto = await _transactionStatusesAppService.GetTransactionStatusForView(id);

            var model = new TransactionStatusViewModel()
            {
                TransactionStatus = getTransactionStatusForViewDto.TransactionStatus
            };

            return PartialView("_ViewTransactionStatusModal", model);
        }

    }
}