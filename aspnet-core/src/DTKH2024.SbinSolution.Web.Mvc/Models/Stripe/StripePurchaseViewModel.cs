﻿using DTKH2024.SbinSolution.MultiTenancy.Payments.Dto;
using DTKH2024.SbinSolution.MultiTenancy.Payments.Stripe;

namespace DTKH2024.SbinSolution.Web.Models.Stripe
{
    public class StripePurchaseViewModel
    {
        public SubscriptionPaymentDto Payment { get; set; }
        
        public decimal Amount { get; set; }

        public bool IsRecurring { get; set; }
        
        public bool IsProrationPayment { get; set; }

        public string SessionId { get; set; }

        public StripePaymentGatewayConfiguration Configuration { get; set; }
    }
}
