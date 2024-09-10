using Abp.AutoMapper;
using DTKH2024.SbinSolution.MultiTenancy.Dto;

namespace DTKH2024.SbinSolution.Mobile.MAUI.Models.Tenants
{
    [AutoMapFrom(typeof(TenantListDto))]
    [AutoMapTo(typeof(TenantEditDto), typeof(CreateTenantInput))]
    public class TenantListModel : TenantListDto
    {
 
    }
}
