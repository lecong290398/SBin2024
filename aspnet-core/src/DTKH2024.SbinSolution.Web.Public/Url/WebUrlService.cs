using Abp.Dependency;
using DTKH2024.SbinSolution.Configuration;
using DTKH2024.SbinSolution.Url;
using DTKH2024.SbinSolution.Web.Url;

namespace DTKH2024.SbinSolution.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}