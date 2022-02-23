
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
    public class CaseStudyPagePreview : CaseStudyPage
    {
        public IEnumerable<PreviewContentError> Errors { get; }

        public CaseStudyPagePreview(string title, string url, Scheme scheme, HtmlString content, IEnumerable<PreviewContentError> errors)
                    : base(title, url, scheme, content)
        {
            Errors = errors;
        }
        public CaseStudyPagePreview(CaseStudyPage caseStudyPage, IEnumerable<PreviewContentError> errors)
                    : base(caseStudyPage.Title, caseStudyPage.Url, caseStudyPage.Scheme, caseStudyPage.Content)
        {
            Errors = errors;
        }
    }
}
