using Abp.AspNetCore.Mvc.Authorization;
using DTKH2024.SbinSolution.Authorization.Users.Profile;
using DTKH2024.SbinSolution.Graphics;
using DTKH2024.SbinSolution.Storage;

namespace DTKH2024.SbinSolution.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService,
            IImageValidator imageValidator) :
            base(tempFileCacheManager, profileAppService, imageValidator)
        {
        }
    }
}