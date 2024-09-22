using DTKH2024.SbinSolution.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DTKH2024.SbinSolution.Web.Public.Controllers
{
    public class SupportController : SbinSolutionControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTicket()
        {
            return View();
        }
    }
}
