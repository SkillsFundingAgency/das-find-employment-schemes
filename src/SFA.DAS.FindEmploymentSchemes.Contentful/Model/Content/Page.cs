using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    //todo: interface?
    public class Page
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public HtmlString Content { get; set; }

        public Page(string title, string url, HtmlString content)
        {
            Title = title;
            Url = url;
            Content = content;
        }
    }
}
