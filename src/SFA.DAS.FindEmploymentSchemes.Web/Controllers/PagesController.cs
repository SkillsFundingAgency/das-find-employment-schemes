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

        public IActionResult Page(string pageUrl)
        {
            pageUrl = pageUrl.ToLowerInvariant();
            switch (pageUrl)
            {
                case "cookies":
                    return View("Cookies");
                case "accessibility-statement":
                    return View("AccessibilityStatement");
                case "error-check":
                    throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
            }

            var page = SchemesContent.Pages.FirstOrDefault(p => p.Url == pageUrl);
            if (page == null)
                return NotFound();

            return View(page);
        }
    }
}
