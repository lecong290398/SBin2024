using System.Threading.Tasks;
using Abp.Application.Services;
using DTKH2024.SbinSolution.MultiTenancy.Dto;
using DTKH2024.SbinSolution.MultiTenancy.Payments.Dto;

namespace DTKH2024.SbinSolution.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
        
        Task<long> StartExtendSubscription(StartExtendSubscriptionInput input);
        
        Task<StartUpgradeSubscriptionOutput> StartUpgradeSubscription(StartUpgradeSubscriptionInput input);
        
        Task<long> StartTrialToBuySubscription(StartTrialToBuySubscriptionInput input);
    }
}
