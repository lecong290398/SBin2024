using DTKH2024.SbinSolution.Dto;

namespace DTKH2024.SbinSolution.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
