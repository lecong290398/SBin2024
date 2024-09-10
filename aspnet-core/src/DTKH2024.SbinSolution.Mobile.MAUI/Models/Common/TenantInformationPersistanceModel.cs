using Abp.AutoMapper;
using DTKH2024.SbinSolution.ApiClient;

namespace DTKH2024.SbinSolution.Models.Common
{
    [AutoMapFrom(typeof(TenantInformation)),
     AutoMapTo(typeof(TenantInformation))]
    public class TenantInformationPersistanceModel
    {
        public string TenancyName { get; set; }

        public int TenantId { get; set; }
    }
}