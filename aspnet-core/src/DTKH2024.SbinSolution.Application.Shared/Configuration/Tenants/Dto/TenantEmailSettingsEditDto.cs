using Abp.Auditing;
using DTKH2024.SbinSolution.Configuration.Dto;

namespace DTKH2024.SbinSolution.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}