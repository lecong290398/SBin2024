using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;

namespace DTKH2024.SbinSolution.Authorization.Accounts.Dto
{
    public class IsTenantAvailableInput
    {
        [Required]
        [MaxLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
    }
}