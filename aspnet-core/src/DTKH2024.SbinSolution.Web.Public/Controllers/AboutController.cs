using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Web.Controllers;

namespace DTKH2024.SbinSolution.Web.Public.Controllers
{
    public class AboutController : SbinSolutionControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}