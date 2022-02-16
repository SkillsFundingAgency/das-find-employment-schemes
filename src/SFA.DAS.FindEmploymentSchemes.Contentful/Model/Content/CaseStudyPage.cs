using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
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
