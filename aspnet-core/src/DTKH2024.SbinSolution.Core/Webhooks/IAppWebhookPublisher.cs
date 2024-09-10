using System.Threading.Tasks;
using DTKH2024.SbinSolution.Authorization.Users;

namespace DTKH2024.SbinSolution.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
