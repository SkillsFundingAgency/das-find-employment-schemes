using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class PageService : IPageService
    {
        public (string?, Page?) Page(string pageUrl, IContent content)
        {
            pageUrl = pageUrl.ToLowerInvariant();

            switch (pageUrl)
            {
                case "cookies":
                    Page? analyticsPage = content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "analyticscookies");
                    Page? marketingPage = content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == "marketingcookies");
                    return (analyticsPage == null || marketingPage == null
                        ? default
                        : ("Cookies", new CookiePage(analyticsPage, marketingPage, false)));

                case "error-check":
                    throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
            }

            return (null, content.Pages.FirstOrDefault(p => p?.Url?.ToLowerInvariant() == pageUrl));
        }
    }
}
