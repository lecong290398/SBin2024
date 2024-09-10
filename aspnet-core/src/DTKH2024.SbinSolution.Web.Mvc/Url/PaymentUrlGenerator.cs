using Abp.Dependency;
using Abp.Extensions;
using Abp.Runtime.Session;
using DTKH2024.SbinSolution.Editions;
using DTKH2024.SbinSolution.ExtraProperties;
using DTKH2024.SbinSolution.MultiTenancy.Payments;
using DTKH2024.SbinSolution.Url;

namespace DTKH2024.SbinSolution.Web.Url
{
    public class PaymentUrlGenerator : IPaymentUrlGenerator, ITransientDependency
    {
        private readonly IWebUrlService _webUrlService;

        public PaymentUrlGenerator(
            IWebUrlService webUrlService)
        {
            _webUrlService = webUrlService;
        }

        public string CreatePaymentRequestUrl(SubscriptionPayment subscriptionPayment)
        {
            var webSiteRootAddress = _webUrlService.GetSiteRootAddress();

            var url = webSiteRootAddress.EnsureEndsWith('/') +
                      "Payment/GatewaySelection" +
                      "?paymentId=" + subscriptionPayment.Id;

            return url;
        }
    }
}