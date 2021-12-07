using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Web.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _log;

        public PagesController(ILogger<PagesController> logger)
        {
            _log = logger;
        }

        // we _could_ add cache control to the page content type
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Page(string pageUrl)
        {
            pageUrl = pageUrl.ToLowerInvariant();
            switch (pageUrl)
            {
                case "cookies":
                    return View("Cookies");
                case "accessibility-statement":
                    return View("AccessibilityStatement");
            }

            var page = SchemesContent.Pages.FirstOrDefault(p => p.Url == pageUrl);
            if (page == null)
                return NotFound();

            return View(page);
        }
    }
}
