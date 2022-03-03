
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;


namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class PagesController : Controller
    {
        private readonly IContentService _contentService;
        private readonly IPageService _pageService;

        public PagesController(
            IContentService contentService,
            IPageService pageService)
        {
            _contentService = contentService;
            _pageService = pageService;
        }

        // we _could_ add cache control parameters to the page content type
        [HttpGet]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Page(string pageUrl)
        {
            var (viewName, page) = _pageService.Page(pageUrl, _contentService.Content);

            if (page == null)
                return NotFound();

            return View(viewName, page);
        }

        [HttpGet]
        public async Task<IActionResult> PagePreview(string pageUrl)
        {
            IContent previewContent = await _contentService.UpdatePreview();

            var (viewName, page) = _pageService.Page(pageUrl, previewContent);

            if (page == null)
                return NotFound();

            return View(viewName ?? "Page", page);
        }

        [HttpPost]
        [Route("page/cookies")]
        public IActionResult Cookies(string AnalyticsCookies, string MarketingCookies)
        {
            string host = HttpContext.Request.Host.Host;
            CookieOptions options = new CookieOptions()
            {
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                Domain = (host == "localhost" ? host : $".{host}")
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
