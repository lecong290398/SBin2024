using System.Linq;
using DTKH2024.SbinSolution.MultiTenancy.Payments.Dto;
using DTKH2024.SbinSolution.MultiTenancy.Payments.Paypal;

namespace DTKH2024.SbinSolution.Web.Models.Paypal
{
    public class PayPalPurchaseViewModel
    {
        public SubscriptionPaymentDto Payment { get; set; }

        public decimal Amount { get; set; }

        public PayPalPaymentGatewayConfiguration Configuration { get; set; }

        public string GetDisabledFundingsQueryString()
        {
            if (Configuration.DisabledFundings == null || !Configuration.DisabledFundings.Any())
            {
                return "";
            }

            return "&disable-funding=" + string.Join(',', Configuration.DisabledFundings.ToList());
        }
    }
}
