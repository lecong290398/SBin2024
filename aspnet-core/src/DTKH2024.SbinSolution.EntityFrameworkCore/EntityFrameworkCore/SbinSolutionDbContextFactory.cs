using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using DTKH2024.SbinSolution.Configuration;
using DTKH2024.SbinSolution.Web;

namespace DTKH2024.SbinSolution.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class SbinSolutionDbContextFactory : IDesignTimeDbContextFactory<SbinSolutionDbContext>
    {
        public SbinSolutionDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SbinSolutionDbContext>();

            /*
             You can provide an environmentName parameter to the AppConfigurations.Get method. 
             In this case, AppConfigurations will try to read appsettings.{environmentName}.json.
             Use Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") method or from string[] args to get environment if necessary.
             https://docs.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#args
             */
            var configuration = AppConfigurations.Get(
                WebContentDirectoryFinder.CalculateContentRootFolder(),
                addUserSecrets: true
            );

            SbinSolutionDbContextConfigurer.Configure(builder, configuration.GetConnectionString(SbinSolutionConsts.ConnectionStringName));

            return new SbinSolutionDbContext(builder.Options);
        }
    }
}
