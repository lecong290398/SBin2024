using Abp.EntityHistory;
using DTKH2024.SbinSolution.Authorization.Users;

namespace DTKH2024.SbinSolution.Auditing
{
    /// <summary>
    /// A helper class to store an <see cref="EntityChange"/> and a <see cref="User"/> object.
    /// </summary>
    public class EntityChangeAndUser
    {
        public EntityChange EntityChange { get; set; }

        public User User { get; set; }
    }
}