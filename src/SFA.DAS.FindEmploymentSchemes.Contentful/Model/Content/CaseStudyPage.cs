using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
    public class CaseStudyPage
    {
        public string Title { get; }
        public string Url { get; }
        public Scheme Scheme { get; }
        public HtmlString Content { get; }

        public CaseStudyPage(string title, string url, Scheme scheme, HtmlString content)
        {
            Title = title;
            Url = url;
            Scheme = scheme;
            Content = content;
        }
    }
}
