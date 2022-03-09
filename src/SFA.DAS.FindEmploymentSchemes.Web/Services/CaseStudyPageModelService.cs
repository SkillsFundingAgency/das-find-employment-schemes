using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class CaseStudyPageModelService : ICaseStudyPageModelService
    {
        public IEnumerable<HtmlString> GetErrors(CaseStudyPage caseStudyPage)
        {
            var errors = new List<HtmlString>();

            if (string.IsNullOrWhiteSpace(caseStudyPage.Title))
            {
                errors.Add(new HtmlString("Title must not be blank"));
            }

            // note: schemes without urls will have already been filtered out
            bool hasValidScheme = caseStudyPage.Scheme?.Name != null;
            if (!hasValidScheme)
            {
                errors.Add(new HtmlString("Scheme must be selected and have been given an URL and name before publishing"));
            }
            if (caseStudyPage.Content == null)
            {
                errors.Add(new HtmlString("Content must not be blank"));
            }

            return errors;
        }
    }
}
