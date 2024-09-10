using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using DTKH2024.SbinSolution.Configuration;

namespace DTKH2024.SbinSolution.Test.Base.Configuration
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(SbinSolutionTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
