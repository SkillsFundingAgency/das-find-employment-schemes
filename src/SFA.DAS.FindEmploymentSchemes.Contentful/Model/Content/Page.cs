using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
    public class Page
    {
        /// <summary>
        /// Should always be valid. We filter out pages with invalid urls, as they aren't navigable.
        /// </summary>
        public string Url { get; }

        // these are mandatory in Contentful, and should never be null for a published page
        #region Mandatory in Contentful

        public string? Title { get; }
        public HtmlString? Content { get; }

        #endregion Mandatory in Contentful

        public Page(string? title, string url, HtmlString? content)
        {
            Title = title;
            Url = url;
            Content = content;
        }
    }
}
