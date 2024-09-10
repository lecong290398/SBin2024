﻿using System;
using Abp.Extensions;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.MultiTenancy.Payments;
using DTKH2024.SbinSolution.MultiTenancy.Payments.Dto;
using DTKH2024.SbinSolution.Web.Models.Payment;
using System.Threading.Tasks;

namespace DTKH2024.SbinSolution.Web.Controllers
{
    public class PaymentController : SbinSolutionControllerBase
    {
        private readonly IPaymentAppService _paymentAppService;
        private readonly IPaymentManager _paymentManager;
        
        public PaymentController(
            IPaymentAppService paymentAppService, 
            IPaymentManager paymentManager)
        {
            _paymentAppService = paymentAppService;
            _paymentManager = paymentManager;
        }

        public async Task<IActionResult> GatewaySelection(long paymentId)
        {
            var payment = await _paymentAppService.GetPaymentAsync(paymentId);
            SetTenantIdCookie(payment.TenantId);

            var model = new GatewaySelectionViewModel
            {
                Payment = payment,
                PaymentGateways = _paymentAppService.GetActiveGateways(new GetActiveGatewaysInput()
                {
                    RecurringPaymentsEnabled = payment.IsRecurring
                })
            };

            return View("GatewaySelection", model);
        }

        public async Task<IActionResult> PaymentFailed(long paymentId)
        {
            await _paymentAppService.PaymentFailed(paymentId);
            return View("PaymentFailed");
        }

        public async Task<IActionResult> PaymentSucceed(long paymentId)
        {
            var payment = await _paymentAppService.GetPaymentAsync(paymentId);
            if (payment.SuccessUrl.IsNullOrEmpty())
            {
                return View("PaymentSucceed");
            }

            return Redirect(payment.SuccessUrl + "?paymentId=" + paymentId);
        }
        
        [HttpPost]
        public async Task<JsonResult> UpdatePayment(ContinuePaymentModel model)
        {
            await _paymentAppService.UpdatePayment(new UpdatePaymentDto()
            {
                PaymentId = model.PaymentId,
                Gateway = model.Gateway,
                IsRecurring = model.RecurringPaymentEnabled
            });
            
            return Json(new AjaxResponse
            {
                TargetUrl = Url.Action("PrePayment", model.Gateway, new
                {
                    paymentId = model.PaymentId
                })
            });
        }
    }
}