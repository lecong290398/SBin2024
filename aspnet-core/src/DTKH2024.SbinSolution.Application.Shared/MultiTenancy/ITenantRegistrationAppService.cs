using System.Threading.Tasks;
using Abp.Application.Services;
using DTKH2024.SbinSolution.Editions.Dto;
using DTKH2024.SbinSolution.MultiTenancy.Dto;

namespace DTKH2024.SbinSolution.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
        
        Task BuyNowSucceed(long paymentId);

        Task NewRegistrationSucceed(long paymentId);

        Task UpgradeSucceed(long paymentId);

        Task ExtendSucceed(long paymentId);
    }
}