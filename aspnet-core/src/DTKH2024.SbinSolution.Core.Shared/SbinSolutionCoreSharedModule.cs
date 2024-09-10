using Abp.Modules;
using Abp.Reflection.Extensions;

namespace DTKH2024.SbinSolution
{
    public class SbinSolutionCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SbinSolutionCoreSharedModule).GetAssembly());
        }
    }
}