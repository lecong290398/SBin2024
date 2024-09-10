using DTKH2024.SbinSolution.MultiTenancy.Dto;
using DTKH2024.SbinSolution.Sessions.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Editions
{
    public class SubscriptionDashboardViewModel
    {
        public GetCurrentLoginInformationsOutput LoginInformations { get; set; }
        
        public EditionsSelectOutput Editions { get; set; }
    }
}
