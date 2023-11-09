using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {

        [Route("index")]
        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Index()
        {

            return View();

        }

    }

}
