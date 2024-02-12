using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
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

        public InterimPreamble? InterimPreamble { get; }

        public InterimBreadcrumbs? InterimBreadcrumbs { get; }

        public string? Title { get; }

        public HtmlString? Content { get; }

        public Page(string? title, string url, HtmlString? content, InterimPreamble? interimPreamble = null, InterimBreadcrumbs? interimPageBreadcrumbs = null)
        {

            Title = title;

            Url = url;

            InterimPreamble = interimPreamble;

            InterimBreadcrumbs = interimPageBreadcrumbs;

            Content = content;

        }

    }

}
