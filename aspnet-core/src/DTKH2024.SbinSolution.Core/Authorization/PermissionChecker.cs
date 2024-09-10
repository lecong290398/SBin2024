using Abp.Authorization;
using DTKH2024.SbinSolution.Authorization.Roles;
using DTKH2024.SbinSolution.Authorization.Users;

namespace DTKH2024.SbinSolution.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
