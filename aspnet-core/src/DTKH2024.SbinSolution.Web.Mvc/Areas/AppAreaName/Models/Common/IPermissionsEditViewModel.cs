using System.Collections.Generic;
using DTKH2024.SbinSolution.Authorization.Permissions.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}