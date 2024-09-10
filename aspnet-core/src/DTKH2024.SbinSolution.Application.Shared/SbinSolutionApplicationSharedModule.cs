using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DTKH2024.SbinSolution
{
    [DependsOn(typeof(SbinSolutionCoreSharedModule))]
    public class SbinSolutionApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SbinSolutionApplicationSharedModule).GetAssembly());
        }
    }
}