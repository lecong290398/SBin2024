using DTKH2024.SbinSolution.Core.Dependency;
using DTKH2024.SbinSolution.Mobile.MAUI.Services.UI;

namespace DTKH2024.SbinSolution.Mobile.MAUI.Shared
{
    public abstract class ModalBase : SbinSolutionComponentBase
    {
        protected ModalManagerService ModalManager { get; set; }

        public abstract string ModalId { get; }

        public ModalBase()
        {
            ModalManager = DependencyResolver.Resolve<ModalManagerService>();
        }

        public virtual async Task Show()
        {
            await ModalManager.Show(JS, ModalId);
            StateHasChanged();
        }

        public virtual async Task Hide()
        {
            await ModalManager.Hide(JS, ModalId);
        }
    }
}
