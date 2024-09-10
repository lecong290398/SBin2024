using System.Collections.Generic;
using DTKH2024.SbinSolution.Authorization.Permissions.Dto;

namespace DTKH2024.SbinSolution.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}