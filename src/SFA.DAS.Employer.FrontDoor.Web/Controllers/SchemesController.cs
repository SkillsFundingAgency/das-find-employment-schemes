using Microsoft.AspNetCore.Mvc;


namespace SFA.DAS.Employer.FrontDoor.Web.Controllers
{
    [Route("schemes")]
    public class SchemesController : Controller
    {
        [Route("apprenticeships")]
        public IActionResult Apprenticeships()
        {
            return View();
        }

    }
}
