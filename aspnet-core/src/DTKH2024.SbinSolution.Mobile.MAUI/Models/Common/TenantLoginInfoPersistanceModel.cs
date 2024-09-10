using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using DTKH2024.SbinSolution.Sessions.Dto;

namespace DTKH2024.SbinSolution.Models.Common
{
    [AutoMapFrom(typeof(TenantLoginInfoDto)),
     AutoMapTo(typeof(TenantLoginInfoDto))]
    public class TenantLoginInfoPersistanceModel : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}