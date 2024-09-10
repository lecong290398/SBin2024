using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using DTKH2024.SbinSolution.Configure;
using DTKH2024.SbinSolution.Startup;
using DTKH2024.SbinSolution.Test.Base;

namespace DTKH2024.SbinSolution.GraphQL.Tests
{
    [DependsOn(
        typeof(SbinSolutionGraphQLModule),
        typeof(SbinSolutionTestBaseModule))]
    public class SbinSolutionGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SbinSolutionGraphQLTestModule).GetAssembly());
        }
    }
}