using Abp.Application.Services.Dto;
using Abp.Webhooks;
using DTKH2024.SbinSolution.WebHooks.Dto;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
