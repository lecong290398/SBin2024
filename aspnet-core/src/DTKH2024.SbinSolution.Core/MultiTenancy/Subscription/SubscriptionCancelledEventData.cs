using Abp.Events.Bus;

namespace DTKH2024.SbinSolution.MultiTenancy.Subscription
{
    public class SubscriptionCancelledEventData : EventData
    {
        public long PaymentId { get; set; }
        
        public string ExternalPaymentId { get; set; }
    }
}