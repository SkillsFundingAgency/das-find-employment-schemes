using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class CaseStudyPage
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public Scheme Scheme { get; set; }
        public IHtmlContent Content { get; set; }

        public CaseStudyPage(string title, string url, Scheme scheme, IHtmlContent content)
        {
            Title = title;
            Url = url;
            Scheme = scheme;
            Content = content;
        }
    }
}
