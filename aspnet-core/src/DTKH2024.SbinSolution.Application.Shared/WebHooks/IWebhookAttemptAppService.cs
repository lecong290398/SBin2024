﻿using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using DTKH2024.SbinSolution.WebHooks.Dto;

namespace DTKH2024.SbinSolution.WebHooks
{
    public interface IWebhookAttemptAppService
    {
        Task<PagedResultDto<GetAllSendAttemptsOutput>> GetAllSendAttempts(GetAllSendAttemptsInput input);

        Task<ListResultDto<GetAllSendAttemptsOfWebhookEventOutput>> GetAllSendAttemptsOfWebhookEvent(GetAllSendAttemptsOfWebhookEventInput input);
    }
}
