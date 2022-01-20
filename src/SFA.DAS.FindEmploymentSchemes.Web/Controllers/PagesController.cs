
using System;
using System.Collections.Generic;
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
                    Page analyticsPage = SchemesContent.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "analyticscookies");
                    Page marketingPage = SchemesContent.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "marketingcookies");
                    return (analyticsPage == null || marketingPage == null
                                ? NotFound()
                                : (IActionResult)View("Cookies", new CookiePage(analyticsPage, marketingPage, false)));

                case "error-check":
                    throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
            }

            if (page == null)
                return NotFound();

            return View(page);
        }

        [HttpPost]
        [Route("page/cookies")]
        public IActionResult Cookies(string AnalyticsCookies, string MarketingCookies)
        {
            CookieOptions options = new CookieOptions()
            {
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            };

            HttpContext.Response
                       .Cookies
                       .Append("AnalyticsConsent",
                               (AnalyticsCookies == "yes").ToString().ToLower(),
                               options);
            HttpContext.Response
                       .Cookies
                       .Append("MarketingCookieConsent",
                               (MarketingCookies == "yes").ToString().ToLower(),
                               options);

            Page analyticsPage = SchemesContent.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "analyticscookies");
            Page marketingPage = SchemesContent.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "marketingcookies");
            return (analyticsPage == null || marketingPage == null
                        ? NotFound()
                        : (IActionResult)View("Cookies", new CookiePage(analyticsPage, marketingPage, true)));
        }
    }
}
