using Abp.Auditing;
using DTKH2024.SbinSolution.Authorization.Users;

namespace DTKH2024.SbinSolution.Auditing
{
    /// <summary>
    /// A helper class to store an <see cref="AuditLog"/> and a <see cref="User"/> object.
    /// </summary>
    public class AuditLogAndUser
    {
        public AuditLog AuditLog { get; set; }

        public User User { get; set; }
    }
}