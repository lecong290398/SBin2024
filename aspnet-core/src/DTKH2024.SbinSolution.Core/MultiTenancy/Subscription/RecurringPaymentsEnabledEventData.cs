using Abp.Events.Bus;

namespace DTKH2024.SbinSolution.MultiTenancy.Subscription
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}