using DTKH2024.SbinSolution.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DTKH2024.SbinSolution.Web.Public.Controllers
{
    public class ShopController : SbinSolutionControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
