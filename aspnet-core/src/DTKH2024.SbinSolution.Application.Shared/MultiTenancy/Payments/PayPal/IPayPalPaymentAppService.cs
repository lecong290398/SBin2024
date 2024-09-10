using System.Threading.Tasks;
using Abp.Application.Services;
using DTKH2024.SbinSolution.MultiTenancy.Payments.PayPal.Dto;

namespace DTKH2024.SbinSolution.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
