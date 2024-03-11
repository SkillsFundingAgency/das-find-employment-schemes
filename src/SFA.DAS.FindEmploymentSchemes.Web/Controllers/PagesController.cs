using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
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
            var pageModel = _pageService.GetPageModel(pageUrl);

            if (pageModel == null)
            {

                return NotFound();

            }
                
            return View(pageModel.ViewName, pageModel);
        }

        [HttpGet]
        public async Task<IActionResult> PagePreview(string pageUrl)
        {
            var (routeName, routeValues) = _pageService.RedirectPreview(pageUrl);
            if (routeName != null)
                return RedirectToRoute(routeName, routeValues);

            var pageModel = await _pageService.GetPageModelPreview(pageUrl);

            if (pageModel == null)
                return NotFound();

            return View(pageModel.ViewName, pageModel);
        }

        [HttpPost]
        [Route("page/cookies")]
        public IActionResult Cookies(string AnalyticsCookies, string MarketingCookies)
        {

            ViewData["Title"] = $"Find training and employment schemes for your business - Cookies";

            SetCookies(AnalyticsCookies, MarketingCookies);

            var cookiePageModel = _pageService.GetCookiePageModel(_contentService.Content, true);
            if (cookiePageModel == null)
                return NotFound();

            return View("Cookies", cookiePageModel);
        }

        [HttpPost]
        [Route("preview/page/cookies")]
        public async Task<IActionResult> CookiesPreview(string AnalyticsCookies, string MarketingCookies)
        {
            SetCookies(AnalyticsCookies, MarketingCookies);

             var pageModel = await _pageService.GetPageModelPreview("cookies");

            if (pageModel == null)
                return NotFound();

            return View(pageModel.ViewName, pageModel);
        }

        private void SetCookies(string analyticsCookies, string marketingCookies)
        {
            string host = HttpContext.Request.Host.Host;
            var options = new CookieOptions //NOSONAR we require access to these cookies in client-side code, so setting HttpOnly would break functionality 
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
                    (analyticsCookies == "yes").ToString().ToLower(),
                    options);
            HttpContext.Response
                .Cookies
                .Append("MarketingCookieConsent",
                    (marketingCookies == "yes").ToString().ToLower(),
                    options);
        }
    }
}
