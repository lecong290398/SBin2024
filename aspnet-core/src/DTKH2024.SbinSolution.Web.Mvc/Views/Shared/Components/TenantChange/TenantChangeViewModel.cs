using Abp.AutoMapper;
using DTKH2024.SbinSolution.Sessions.Dto;

namespace DTKH2024.SbinSolution.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}