using Microsoft.Extensions.DependencyInjection;
using DTKH2024.SbinSolution.HealthChecks;

namespace DTKH2024.SbinSolution.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<SbinSolutionDbContextHealthCheck>("Database Connection");
            builder.AddCheck<SbinSolutionDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
