using System.Collections.Generic;
using System.Linq;
using Abp.AutoMapper;
using DTKH2024.SbinSolution.MultiTenancy.Dto;
using DTKH2024.SbinSolution.MultiTenancy.Payments;

namespace DTKH2024.SbinSolution.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
        public List<PaymentPeriodType> GetAvailablePaymentPeriodTypes()
        {
            var result = new List<PaymentPeriodType>();
            
            if (EditionsWithFeatures.Any(e=> e.Edition.MonthlyPrice.HasValue))
            {
                result.Add(PaymentPeriodType.Monthly);
            }
            
            if (EditionsWithFeatures.Any(e=> e.Edition.AnnualPrice.HasValue))
            {
                result.Add(PaymentPeriodType.Annual);
            }
            
            return result;
        } 
    }
}
