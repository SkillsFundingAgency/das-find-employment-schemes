
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;


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
        [HttpGet]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Page(string pageUrl)
        {
            pageUrl = pageUrl.ToLowerInvariant();
            Page page = SchemesContent.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == pageUrl);

            switch (pageUrl)
            {
                case "cookies":
                    return (page == null ? NotFound() : (IActionResult)View("Cookies", new CookiePage(page, false)));

                case "error-check":
                    throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
            }

            if (page == null)
                return NotFound();

            return View(page);
        }

        [HttpPost]
        [Route("page/Cookie")]
        public IActionResult Cookie(string AnalyticsCookies)
        {
            HttpContext.Response
                       .Cookies
                       .Append("AnalyticsConsent",
                               (AnalyticsCookies == "yes").ToString().ToLower(),
                               new CookieOptions() {
                                   Secure = true,
                                   Expires = DateTimeOffset.UtcNow.AddYears(1)
                               });

            Page page = SchemesContent.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "cookies");
            return (page == null
                        ? NotFound()
                        : (IActionResult)View("Cookies", new CookiePage(page, true)));
        }
    }
}
