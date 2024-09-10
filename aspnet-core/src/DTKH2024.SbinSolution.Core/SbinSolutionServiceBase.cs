using Abp;

namespace DTKH2024.SbinSolution
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="SbinSolutionDomainServiceBase"/>.
    /// For application services inherit SbinSolutionAppServiceBase.
    /// </summary>
    public abstract class SbinSolutionServiceBase : AbpServiceBase
    {
        protected SbinSolutionServiceBase()
        {
            LocalizationSourceName = SbinSolutionConsts.LocalizationSourceName;
        }
    }
}