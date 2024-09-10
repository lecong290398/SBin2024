using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace DTKH2024.SbinSolution.Localization
{
    public static class SbinSolutionLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    SbinSolutionConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(SbinSolutionLocalizationConfigurer).GetAssembly(),
                        "DTKH2024.SbinSolution.Localization.SbinSolution"
                    )
                )
            );
        }
    }
}