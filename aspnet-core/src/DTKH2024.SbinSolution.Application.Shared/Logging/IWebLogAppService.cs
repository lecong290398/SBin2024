using Abp.Application.Services;
using DTKH2024.SbinSolution.Dto;
using DTKH2024.SbinSolution.Logging.Dto;

namespace DTKH2024.SbinSolution.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
