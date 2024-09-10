using System.Collections.Generic;
using Abp.Dependency;

namespace DTKH2024.SbinSolution.DashboardCustomization.Definitions.Cache
{
    public interface IWidgetFilterDefinitionCacheManager : ITransientDependency
    {
        List<WidgetFilterDefinition> GetAll();
        
        void Set(List<WidgetFilterDefinition> definition);
    }
}