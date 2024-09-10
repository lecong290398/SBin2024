using System.Threading.Tasks;
using Abp.Application.Services;
using DTKH2024.SbinSolution.Install.Dto;

namespace DTKH2024.SbinSolution.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}