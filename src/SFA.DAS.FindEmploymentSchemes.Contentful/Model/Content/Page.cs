using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
    public class Page
    {
        // all of these are mandatory in Contentful, and should never be null for a published page.
        // all can be null when previewing a page, except we filter out pages with null urls, as they can't be previewed due to routing.
        public string? Title { get; }
        public string Url { get; }
        public HtmlString? Content { get; }

        public Page(string? title, string url, HtmlString? content)
        {
            Title = title;
            Url = url;
            Content = content;
        }
    }
}
