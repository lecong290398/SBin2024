using System.Threading.Tasks;
using Abp.Webhooks;

namespace DTKH2024.SbinSolution.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
