using Abp.Domain.Services;

namespace DTKH2024.SbinSolution
{
    public abstract class SbinSolutionDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected SbinSolutionDomainServiceBase()
        {
            LocalizationSourceName = SbinSolutionConsts.LocalizationSourceName;
        }
    }
}
