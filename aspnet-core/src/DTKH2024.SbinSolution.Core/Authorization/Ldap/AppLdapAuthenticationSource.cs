using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using DTKH2024.SbinSolution.Authorization.Users;
using DTKH2024.SbinSolution.MultiTenancy;

namespace DTKH2024.SbinSolution.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}