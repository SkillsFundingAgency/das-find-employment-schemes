using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
    public class Page
    {
        //todo: change all content fields to nullable
        public string Title { get; }
        public string Url { get; }
        public HtmlString Content { get; }

        public Page(string title, string url, HtmlString content)
        {
            Title = title;
            Url = url;
            Content = content;
        }
    }
}
