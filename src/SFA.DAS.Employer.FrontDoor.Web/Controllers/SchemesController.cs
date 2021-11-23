using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Employer.FrontDoor.Web.Models;

namespace SFA.DAS.Employer.FrontDoor.Web.Controllers
{
    public class SchemesController : Controller
    {
        private readonly ILogger<SchemesController> _logger;

        //todo: rename
        private static readonly HomeModel HomeModel = new HomeModel();

        public SchemesController(ILogger<SchemesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Home()
        {
            return View(HomeModel);
        }

        public IActionResult Details(string schemeUrl)
        {
            var scheme = HomeModel.Schemes.FirstOrDefault(s => s.Url == schemeUrl);
            if (scheme == null)
                return NotFound();

            return View(scheme);
        }
    }
}
