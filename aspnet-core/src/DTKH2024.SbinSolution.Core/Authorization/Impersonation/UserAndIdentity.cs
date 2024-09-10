using System.Security.Claims;
using DTKH2024.SbinSolution.Authorization.Users;

namespace DTKH2024.SbinSolution.Authorization.Impersonation
{
    public class UserAndIdentity
    {
        public User User { get; set; }

        public ClaimsIdentity Identity { get; set; }

        public UserAndIdentity(User user, ClaimsIdentity identity)
        {
            User = user;
            Identity = identity;
        }
    }
}