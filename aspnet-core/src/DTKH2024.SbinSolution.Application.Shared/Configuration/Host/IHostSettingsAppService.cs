using System.Threading.Tasks;
using Abp.Application.Services;
using DTKH2024.SbinSolution.Configuration.Host.Dto;

namespace DTKH2024.SbinSolution.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
