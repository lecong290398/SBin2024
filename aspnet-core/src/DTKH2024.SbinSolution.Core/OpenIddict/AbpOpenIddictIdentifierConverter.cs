using Abp.Dependency;
using System;

namespace DTKH2024.SbinSolution.OpenIddict
{
    public class AbpOpenIddictIdentifierConverter : ITransientDependency
    {
        public virtual Guid FromString(string identifier)
        {
            return string.IsNullOrEmpty(identifier) ? default : Guid.Parse(identifier);
        }

        public virtual string ToString(Guid identifier)
        {
            return identifier.ToString("D");
        }
    }

}
