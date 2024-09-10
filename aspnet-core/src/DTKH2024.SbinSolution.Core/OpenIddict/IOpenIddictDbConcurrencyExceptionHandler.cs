using System.Threading.Tasks;
using Abp.Domain.Uow;

namespace DTKH2024.SbinSolution.OpenIddict
{
    public interface IOpenIddictDbConcurrencyExceptionHandler
    {
        Task HandleAsync(AbpDbConcurrencyException exception);
    }
}