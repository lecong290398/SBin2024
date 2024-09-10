using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using DTKH2024.SbinSolution.ApiClient;
using DTKH2024.SbinSolution.Mobile.MAUI.Core.ApiClient;

namespace DTKH2024.SbinSolution
{
    [DependsOn(typeof(SbinSolutionClientModule), typeof(AbpAutoMapperModule))]

    public class SbinSolutionMobileMAUIModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.ReplaceService<IApplicationContext, MAUIApplicationContext>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SbinSolutionMobileMAUIModule).GetAssembly());
        }
    }
}