using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using DTKH2024.SbinSolution.EntityFrameworkCore;

namespace DTKH2024.SbinSolution.HealthChecks
{
    public class SbinSolutionDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public SbinSolutionDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("SbinSolutionDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("SbinSolutionDbContext could not connect to database"));
        }
    }
}
