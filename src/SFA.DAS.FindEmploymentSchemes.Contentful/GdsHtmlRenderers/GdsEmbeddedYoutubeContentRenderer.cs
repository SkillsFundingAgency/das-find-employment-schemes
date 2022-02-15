﻿using Contentful.Core.Models;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.GdsHtmlRenderers
{
    /// <summary>
    /// A renderer for an embedded YouTube video
    /// </summary>
    public class GdsEmbeddedYoutubeContentRenderer : IContentRenderer
    {
        /// <summary>
        /// The order of this renderer in the collection.
        /// </summary>
        public int Order { get; set; } = 30;

        /// <summary>
        /// Whether or not this renderer supports the provided content.
        /// </summary>
        /// <param name="content">The content to evaluate.</param>
        /// <returns>Returns true if the content is a paragraph, contains only an iframe and refers to a youtube embedded url, otherwise false.</returns>
        public bool SupportsContent(IContent content)
        {
            if (!(content is Paragraph))
                return false;

            Paragraph paragraph = (Paragraph)content;
            if (paragraph.Content.Count != 1 || !(paragraph.Content[0] is Text))
                return false;

            string text = ((Text)paragraph.Content[0])
                                          .Value
                                          .Trim();
            return text.StartsWith("<iframe") && text.EndsWith("</iframe>") &&
                   (text.Contains("youtube.com/embed/") || text.Contains("youtube-nocookie.com/embed/"));
        }

        //todo: this doesn't work

        /// <summary>
        /// Renders the content raw inside an html p-tag
        /// </summary>
        /// <param name="content">The content to render.</param>
        /// <returns>The p-tag as a string.</returns>
        public string Render(IContent content)
        {
            var paragraph = content as Paragraph;
            var sb = new StringBuilder();
            sb.Append("<p class=\"govuk-body\">");

            foreach (var subContent in paragraph!.Content)
            {
                sb.Append(((Text)paragraph.Content[0])
                                          .Value
                                          .Replace("youtube.com/embed/", "youtube-nocookie.com/embed/", System.StringComparison.InvariantCultureIgnoreCase));
            }

            sb.Append("</p>");
            return sb.ToString();
        }

        /// <summary>
        /// Renders the content asynchronously.
        /// </summary>
        /// <param name="content">The content to render.</param>
        /// <returns>The rendered string.</returns>
        public Task<string> RenderAsync(IContent content)
        {
            return Task.FromResult(Render(content));
        }
    }
}