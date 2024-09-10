using System.Threading.Tasks;
using Abp.Application.Services;
using DTKH2024.SbinSolution.MultiTenancy.Payments.Dto;
using DTKH2024.SbinSolution.MultiTenancy.Payments.Stripe.Dto;

namespace DTKH2024.SbinSolution.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();
        
        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}