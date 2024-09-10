using Abp.AutoMapper;
using DTKH2024.SbinSolution.MultiTenancy.Dto;

namespace DTKH2024.SbinSolution.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}