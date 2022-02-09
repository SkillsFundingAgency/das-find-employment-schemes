using Contentful.Core.Models;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.GdsHtmlRenderers
{
    /// <summary>
    /// A renderer for a block quote
    /// </summary>
    public class GdsBlockQuoteRenderer : IContentRenderer
    {
        private readonly ContentRendererCollection _rendererCollection;

        /// <summary>
        /// Initializes a new GdsBlockQuoteRenderer
        /// </summary>
        /// <param name="rendererCollection">The collection of renderer to use for sub-content.</param>
        public GdsBlockQuoteRenderer(ContentRendererCollection rendererCollection)
        {
            _rendererCollection = rendererCollection;
        }

        /// <summary>
        /// The order of this renderer in the collection.
        /// </summary>
        public int Order { get; set; } = 50;

        /// <summary>
        /// Whether or not this renderer supports the provided content.
        /// </summary>
        /// <param name="content">The content to evaluate.</param>
        /// <returns>Returns true if the content is a quote, otherwise false.</returns>
        public bool SupportsContent(IContent content)
        {
            return content is Quote;

            //if (!(content is Paragraph))
            //    return false;

            //Paragraph paragraph = (Paragraph)content;
            //if (paragraph.Content.Count != 3 || !(paragraph.Content[0] is Text) || !(paragraph.Content[1] is Text) || !(paragraph.Content[2] is Text))
            //    return false;

            //return ((Text)paragraph.Content[0])
            //                       .Value
            //                       .Trim()
            //                       .StartsWith("\"");
        }

        /// <summary>
        /// Renders the content to a string.
        /// </summary>
        /// <param name="content">The content to render.</param>
        /// <returns>The block quote as a quote HTML string.</returns>
        public string Render(IContent content)
        {
            var quote = content as Quote;

            var sb = new StringBuilder();

            sb.Append("<div class=\"govuk-inset-text quote-and-attribution\">");

            foreach (var subContent in quote!.Content)
            {
                var renderer = _rendererCollection.GetRendererForContent(subContent);
                sb.Append(renderer.Render(subContent));
            }

            sb.Append("</div>");

            return sb.ToString();

            //var paragraph = content as Paragraph;
            //if (paragraph == null)
            //    return "";

            //var sb = new StringBuilder();
            //sb.Append("<p class=\"govuk-body quote-and-attribution\">");

            //string quote = ((Text)paragraph.Content[0]).Value;
            //string name = ((Text)paragraph.Content[1]).Value;
            //string title = ((Text)paragraph.Content[2]).Value;

            //sb.Append($"{quote}<br />");
            //sb.Append($"<b>{name}</b><br />");
            //sb.Append($"{title}");

            //sb.Append("</p>");
            //return sb.ToString();
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
