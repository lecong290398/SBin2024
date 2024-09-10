using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTKH2024.SbinSolution.Authorization.Permissions.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Common.Modals
{
    public class PermissionTreeModalViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }
}
