using Contentful.Core.Models;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.GdsHtmlRenderers
{
    /// <summary>
    /// A renderer for text surrounded with quote marks followed by quote attribution
    /// "These are actual words that I spoke. Don't quote me on that."
    /// Bob Quoter
    /// Manager, Quotes UK Ltd
    /// </summary>
    public class GdsQuotedContentRenderer : IContentRenderer
    {
        private readonly ContentRendererCollection _rendererCollection;

        /// <summary>
        /// Initializes a new GdsQuotedContentRenderer
        /// </summary>
        /// <param name="rendererCollection">The collection of renderer to use for sub-content.</param>
        public GdsQuotedContentRenderer(ContentRendererCollection rendererCollection)
        {
            _rendererCollection = rendererCollection;
        }

        /// <summary>
        /// The order of this renderer in the collection.
        /// </summary>
        public int Order { get; set; } = 40;

        /// <summary>
        /// Whether or not this renderer supports the provided content.
        /// </summary>
        /// <param name="content">The content to evaluate.</param>
        /// <returns>Returns true if the content is a paragraph and contains 3 content items with the first quoted text, otherwise false.</returns>
        public bool SupportsContent(IContent content)
        {
            if (!(content is Paragraph))
                return false;

            Paragraph paragraph = (Paragraph)content;
            if (paragraph.Content.Count != 3 || !(paragraph.Content[0] is Text) || !(paragraph.Content[1] is Text) || !(paragraph.Content[2] is Text))
                return false;

            return ((Text)paragraph.Content[0])
                                   .Value
                                   .Trim()
                                   .StartsWith("\"");
        }

        /// <summary>
        /// Renders the content raw inside an html p-tag
        /// </summary>
        /// <param name="content">The content to render.</param>
        /// <returns>The p-tag as a string.</returns>
        public string Render(IContent content)
        {
            var paragraph = content as Paragraph;
            if (paragraph == null)
                return "";

            var sb = new StringBuilder();
            sb.Append("<p class=\"govuk-body\"><i>");

            string quote = ((Text)paragraph.Content[0]).Value;
            string name = ((Text)paragraph.Content[1]).Value;
            string title = ((Text)paragraph.Content[2]).Value;

            sb.Append($"{quote}<br />");
            sb.Append($"<b>{name}</b><br />");
            sb.Append($"{title}");

            sb.Append("</i></p>");
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