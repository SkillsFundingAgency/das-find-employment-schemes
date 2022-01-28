using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _log;
        private readonly IContentService _contentService;

        public PagesController(
            ILogger<PagesController> logger,
            IContentService contentService)
        {
            _log = logger;
            _contentService = contentService;
        }

        // we _could_ add cache control parameters to the page content type
        [HttpGet]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Page(string pageUrl)
        {
            pageUrl = pageUrl.ToLowerInvariant();

            switch (pageUrl)
            {
                case "cookies":
                    Page? analyticsPage = _contentService.Content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "analyticscookies");
                    Page? marketingPage = _contentService.Content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "marketingcookies");
                    return (analyticsPage == null || marketingPage == null
                                ? NotFound()
                                : (IActionResult)View("Cookies", new CookiePage(analyticsPage, marketingPage, false)));

                case "error-check":
                    throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
            }

            Page? page = _contentService.Content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == pageUrl);
            if (page == null)
                return NotFound();

            return View(page);
        }

        [HttpPost]
        [Route("page/cookies")]
        public IActionResult Cookies(string AnalyticsCookies, string MarketingCookies)
        {
            var options = new CookieOptions
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

            Page? analyticsPage = _contentService.Content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "analyticscookies");
            Page? marketingPage = _contentService.Content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "marketingcookies");
            return (analyticsPage == null || marketingPage == null
                        ? NotFound()
                        : (IActionResult)View("Cookies", new CookiePage(analyticsPage, marketingPage, true)));
        }
    }
}
