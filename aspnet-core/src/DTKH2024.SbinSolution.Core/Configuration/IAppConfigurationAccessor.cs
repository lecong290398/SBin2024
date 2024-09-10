using Microsoft.Extensions.Configuration;

namespace DTKH2024.SbinSolution.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
