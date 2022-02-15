using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    public class SubScheme
    {
        public string Title { get; }
        public HtmlString? Summary { get; }
        public HtmlString Content { get; }

        public SubScheme(string title, HtmlString? summary, HtmlString content)
        {
            Title = title;
            Summary = summary;
            Content = content;
        }
    }
}
