using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
    public class CaseStudyPage
    {
        /// <summary>
        /// Should always be valid. We filter out case study pages with invalid urls, as they aren't navigable.
        /// </summary>
        public string Url { get; }

        // these are mandatory in Contentful, and should never be null for a published case study page
        #region Mandatory in Contentful

        public string? Title { get; }
        public Scheme? Scheme { get; }
        public HtmlString? Content { get; }

        #endregion Mandatory in Contentful

        public CaseStudyPage(string? title, string url, Scheme? scheme, HtmlString? content)
        {
            Title = title;
            Url = url;
            Scheme = scheme;
            Content = content;
        }
    }
}
