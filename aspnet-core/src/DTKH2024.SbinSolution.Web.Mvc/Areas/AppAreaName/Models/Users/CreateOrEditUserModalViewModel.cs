using System.Linq;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using DTKH2024.SbinSolution.Authorization.Users.Dto;
using DTKH2024.SbinSolution.Security;
using DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Common;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Users
{
    [AutoMapFrom(typeof(GetUserForEditOutput))]
    public class CreateOrEditUserModalViewModel : GetUserForEditOutput, IOrganizationUnitsEditViewModel
    {
        public bool CanChangeUserName => User.UserName != AbpUserBase.AdminUserName;

        public int AssignedRoleCount
        {
            get { return Roles.Count(r => r.IsAssigned); }
        }
        
        public int AssignedOrganizationUnitCount => MemberedOrganizationUnits.Count;

        public bool IsEditMode => User.Id.HasValue;

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }
    }
}