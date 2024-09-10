using Abp.Events.Bus.Handlers;
using DTKH2024.SbinSolution.MultiTenancy.Subscription;

namespace DTKH2024.SbinSolution.MultiTenancy.Payments
{
    public interface ISupportsRecurringPayments : 
        IEventHandler<RecurringPaymentsDisabledEventData>, 
        IEventHandler<RecurringPaymentsEnabledEventData>,
        IEventHandler<SubscriptionUpdatedEventData>,
        IEventHandler<SubscriptionCancelledEventData>
    {

    }
}
