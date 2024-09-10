using Microsoft.AspNetCore.Components;
using DTKH2024.SbinSolution.Authorization.Accounts;
using DTKH2024.SbinSolution.Authorization.Accounts.Dto;
using DTKH2024.SbinSolution.Core.Dependency;
using DTKH2024.SbinSolution.Core.Threading;
using DTKH2024.SbinSolution.Mobile.MAUI.Models.Login;
using DTKH2024.SbinSolution.Mobile.MAUI.Shared;

namespace DTKH2024.SbinSolution.Mobile.MAUI.Pages.Login
{
    public partial class EmailActivationModal : ModalBase
    {
        public override string ModalId => "email-activation-modal";

        [Parameter] public EventCallback OnSave { get; set; }

        public EmailActivationModel emailActivationModel { get; set; } = new EmailActivationModel();

        private readonly IAccountAppService _accountAppService;

        public EmailActivationModal()
        {
            _accountAppService = DependencyResolver.Resolve<IAccountAppService>();
        }

        protected virtual async Task Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(
                async () =>
                    await _accountAppService.SendEmailActivationLink(new SendEmailActivationLinkInput
                    {
                        EmailAddress = emailActivationModel.EmailAddress
                    }),
                    async () =>
                    {
                        await OnSave.InvokeAsync();
                    }
                );
            });
        }

        protected virtual async Task Cancel()
        {
            await Hide();
        }
    }
}
