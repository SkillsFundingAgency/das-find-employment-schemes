using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class CaseStudyPageService : ICaseStudyPageService
    {
        public (string?, CaseStudyPage?) CaseStudyPage(string pageUrl, IContent content)
        {
            pageUrl = pageUrl.ToLowerInvariant();

            switch (pageUrl)
            {
                case "error-check":
                    throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
            }

            //todo: tolower all urls in content when we update content from contentful, then remove these
            return (null, content.CaseStudyPages.FirstOrDefault(p => p.Url.ToLowerInvariant() == pageUrl));
        }
    }
}
