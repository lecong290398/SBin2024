using System.Collections.Generic;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.Authorization.Permissions.Dto;
using DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Common;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}