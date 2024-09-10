using Microsoft.AspNetCore.Components;
using DTKH2024.SbinSolution.Authorization.Accounts;
using DTKH2024.SbinSolution.Authorization.Accounts.Dto;
using DTKH2024.SbinSolution.Core.Dependency;
using DTKH2024.SbinSolution.Core.Threading;
using DTKH2024.SbinSolution.Mobile.MAUI.Models.Login;
using DTKH2024.SbinSolution.Mobile.MAUI.Shared;

namespace DTKH2024.SbinSolution.Mobile.MAUI.Pages.Login
{
    public partial class ForgotPasswordModal : ModalBase
    {
        public override string ModalId => "forgot-password-modal";
       
        [Parameter] public EventCallback OnSave { get; set; }
        
        public ForgotPasswordModel forgotPasswordModel { get; set; } = new ForgotPasswordModel();

        private readonly IAccountAppService _accountAppService;

        public ForgotPasswordModal()
        {
            _accountAppService = DependencyResolver.Resolve<IAccountAppService>();
        }

        protected virtual async Task Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(
                async () =>
                    await _accountAppService.SendPasswordResetCode(new SendPasswordResetCodeInput { EmailAddress = forgotPasswordModel.EmailAddress }),
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
