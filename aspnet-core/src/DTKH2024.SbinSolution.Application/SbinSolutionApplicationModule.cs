using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DTKH2024.SbinSolution.Authorization;

namespace DTKH2024.SbinSolution
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(SbinSolutionApplicationSharedModule),
        typeof(SbinSolutionCoreModule)
        )]
    public class SbinSolutionApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SbinSolutionApplicationModule).GetAssembly());
        }
    }
}