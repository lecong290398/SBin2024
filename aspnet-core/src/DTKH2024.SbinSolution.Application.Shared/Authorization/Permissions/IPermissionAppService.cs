using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization.Permissions.Dto;

namespace DTKH2024.SbinSolution.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
