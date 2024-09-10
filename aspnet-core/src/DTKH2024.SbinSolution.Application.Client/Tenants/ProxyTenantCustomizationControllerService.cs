using DTKH2024.SbinSolution.Authorization.Users.Profile.Dto;
using System.Threading.Tasks;

namespace DTKH2024.SbinSolution.Tenants
{
    public class ProxyTenantCustomizationControllerService : ProxyControllerBase
    {
        public async Task<GetTenantLogoOutput> GetTenantLogoOrNull(int? tenantId, string skin = "light")
        {
            return await ApiClient.GetAnonymousAsync<GetTenantLogoOutput>(GetEndpoint(nameof(GetTenantLogoOrNull)), new { tenantId, skin });
        }
    }
}
