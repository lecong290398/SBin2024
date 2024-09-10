using System.Threading.Tasks;
using Abp.Application.Services;
using DTKH2024.SbinSolution.Sessions.Dto;

namespace DTKH2024.SbinSolution.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
