using Abp.Localization;
using Microsoft.AspNetCore.Components;
using DTKH2024.SbinSolution.ApiClient;
using DTKH2024.SbinSolution.Authorization.Users.Dto;
using DTKH2024.SbinSolution.Authorization.Users.Profile;
using DTKH2024.SbinSolution.Core.Dependency;
using DTKH2024.SbinSolution.Core.Threading;
using DTKH2024.SbinSolution.Mobile.MAUI.Services.Account;
using DTKH2024.SbinSolution.Mobile.MAUI.Services.UI;
using DTKH2024.SbinSolution.Mobile.MAUI.Shared;


namespace DTKH2024.SbinSolution.Mobile.MAUI.Pages.MySettings
{
    public partial class LanguageSwitch : SbinSolutionComponentBase
    {
        protected LanguageService LanguageService { get; set; }

        private IApplicationContext _applicationContext;
        private readonly IProfileAppService _profileAppService;
        private List<LanguageInfo> _languages;
        private string _selectedLanguage;

        [Parameter] public EventCallback OnSave { get; set; }

        public LanguageSwitch()
        {
            _applicationContext = DependencyResolver.Resolve<IApplicationContext>();
            _profileAppService = DependencyResolver.Resolve<IProfileAppService>();
            LanguageService = DependencyResolver.Resolve<LanguageService>();

            _languages = _applicationContext.Configuration.Localization.Languages;
            _selectedLanguage = _languages.FirstOrDefault(l => l.Name == _applicationContext.CurrentLanguage.Name).Name;
        }

        public List<LanguageInfo> Languages
        {
            get => _languages;
            set => _languages = value;
        }

        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                _selectedLanguage = value;
                AsyncRunner.Run(ChangeLanguage());
            }
        }

        private async Task ChangeLanguage()
        {
            var selectedLanguage = _languages?.FirstOrDefault(l => l.Name == _selectedLanguage);
            _applicationContext.CurrentLanguage = selectedLanguage;

            await SetBusyAsync(async () =>
            {
                if (_applicationContext.LoginInfo is null)
                {
                    await UserConfigurationManager.GetAsync();
                    await OnSave.InvokeAsync();
                    LanguageService.ChangeLanguage();

                    return;
                }

                await WebRequestExecuter.Execute(async () =>
                {
                    await _profileAppService.ChangeLanguage(new ChangeUserLanguageDto
                    {
                        LanguageName = _selectedLanguage
                    });
                }, async () =>
                {
                    await UserConfigurationManager.GetAsync();
                    await OnSave.InvokeAsync();
                    LanguageService.ChangeLanguage();
                });
            });
        }
    }
}