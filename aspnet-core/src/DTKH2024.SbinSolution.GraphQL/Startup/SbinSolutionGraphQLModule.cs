using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DTKH2024.SbinSolution.Startup
{
    [DependsOn(typeof(SbinSolutionCoreModule))]
    public class SbinSolutionGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SbinSolutionGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}