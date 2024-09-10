using System.Collections.Generic;
using DTKH2024.SbinSolution.Authorization.Permissions.Dto;

namespace DTKH2024.SbinSolution.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}