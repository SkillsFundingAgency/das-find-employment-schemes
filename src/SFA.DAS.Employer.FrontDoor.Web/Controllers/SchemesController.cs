using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.Employer.FrontDoor.Web.Controllers
{
    public class SchemesController : Controller
    {
        public IActionResult Details(string schemeUrl)
        {
            return View();
        }
    }
}
