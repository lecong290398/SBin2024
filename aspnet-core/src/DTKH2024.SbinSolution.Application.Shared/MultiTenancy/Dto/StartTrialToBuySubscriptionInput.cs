using DTKH2024.SbinSolution.MultiTenancy.Payments;

namespace DTKH2024.SbinSolution.MultiTenancy.Dto
{
    public class StartTrialToBuySubscriptionInput
    {
        public PaymentPeriodType PaymentPeriodType { get; set; }
        
        public string SuccessUrl { get; set; }

        public string ErrorUrl { get; set; }
    }
}