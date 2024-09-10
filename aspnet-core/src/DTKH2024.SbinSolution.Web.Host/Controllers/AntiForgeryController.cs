using Microsoft.AspNetCore.Antiforgery;

namespace DTKH2024.SbinSolution.Web.Controllers
{
    public class AntiForgeryController : SbinSolutionControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
