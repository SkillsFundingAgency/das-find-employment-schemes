using System;
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

        // we _could_ add cache control parameters to the page content type
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Page(string pageUrl)
        {
            pageUrl = pageUrl.ToLowerInvariant();
            var page = SchemesContent.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == pageUrl);

            switch (pageUrl)
            {
                case "cookies":
                    return (page == null ? NotFound() : (IActionResult)View("Cookies", page));

                case "accessibility-statement":
                    return View("AccessibilityStatement");

                case "error-check":
                    throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
            }

            if (page == null)
                return NotFound();

            return View(page);
        }
    }
}
