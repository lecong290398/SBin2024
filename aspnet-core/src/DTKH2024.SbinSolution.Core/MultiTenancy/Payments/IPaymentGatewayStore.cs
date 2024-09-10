using System.Collections.Generic;

namespace DTKH2024.SbinSolution.MultiTenancy.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
