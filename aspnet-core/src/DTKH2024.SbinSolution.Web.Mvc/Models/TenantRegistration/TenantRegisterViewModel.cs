using DTKH2024.SbinSolution.Editions;
using DTKH2024.SbinSolution.Editions.Dto;
using DTKH2024.SbinSolution.MultiTenancy.Payments;
using DTKH2024.SbinSolution.Security;
using DTKH2024.SbinSolution.MultiTenancy.Payments.Dto;

namespace DTKH2024.SbinSolution.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public int? EditionId { get; set; }

        public EditionSelectDto Edition { get; set; }
        
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
        
        public SubscriptionStartType? SubscriptionStartType { get; set; }
        
        public PaymentPeriodType? PaymentPeriodType { get; set; }
        
        public string SuccessUrl { get; set; }
        
        public string ErrorUrl { get; set; }
    }
}
