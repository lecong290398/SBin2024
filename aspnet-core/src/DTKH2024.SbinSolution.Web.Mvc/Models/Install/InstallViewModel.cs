using System.Collections.Generic;
using Abp.Localization;
using DTKH2024.SbinSolution.Install.Dto;

namespace DTKH2024.SbinSolution.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
