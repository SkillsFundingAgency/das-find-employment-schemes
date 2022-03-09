using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class PageService : IPageService
    {
        private const string CookiesPageUrl = "cookies";
        private const string AnalyticsCookiesPageUrl = "analyticscookies";
        private const string MarketingCookiesPageUrl = "marketingcookies";

        public (string?, Page?) Page(string pageUrl, IContent content)
        {
            pageUrl = pageUrl.ToLowerInvariant();

            switch (pageUrl)
            {
                case CookiesPageUrl:
                    Page? analyticsPage = content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == AnalyticsCookiesPageUrl);
                    Page? marketingPage = content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == MarketingCookiesPageUrl);
                    if (analyticsPage == null || marketingPage == null)
                        throw new ContentServiceException($"Missing {AnalyticsCookiesPageUrl} and/or {MarketingCookiesPageUrl} page(s).");

                    return ("Cookies", new CookiePage(analyticsPage, marketingPage, false));

                case "error-check":
                    throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
            }

            //todo: enforce lowercase url in contentful and contentservice
            return (null, content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == pageUrl));
        }

        public (string? routeName, object? routeValues) RedirectPreview(string pageUrl)
        {
            pageUrl = pageUrl.ToLowerInvariant();

            switch (pageUrl)
            {
                case AnalyticsCookiesPageUrl:
                case MarketingCookiesPageUrl:
                    return ("page-preview", new { pageUrl = CookiesPageUrl });
                case "home":
                    return ("home-preview", null);
            }

            return default;
        }
    }
}
