using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contentful.Core.Models;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.GdsHtmlRenderers
{
    /// <summary>
    /// A renderer for a GDS compliant hyperlink.
    /// </summary>
    public class GdsHyperlinkContentRenderer : IContentRenderer
    {
        private readonly ContentRendererCollection _rendererCollection;

        /// <summary>
        /// Initializes a new GdsHyperlinkContentRenderer.
        /// </summary>
        /// <param name="rendererCollection">The collection of renderer to use for sub-content.</param>
        public GdsHyperlinkContentRenderer(ContentRendererCollection rendererCollection)
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
        /// <returns>Returns true if the content is a hyperlink, otherwise false.</returns>
        public bool SupportsContent(IContent content)
        {
            return content is Hyperlink;
        }

        /// <summary>
        /// Renders the content to a string.
        /// </summary>
        /// <param name="content">The content to render.</param>
        /// <returns>The GDS compliant a tag as a string.</returns>
        public string Render(IContent content)
        {
            var link = content as Hyperlink;
            var sb = new StringBuilder();

            // we assume we only get asked to render what we've said we support
            sb.Append($"<a href=\"{link!.Data.Uri}\" title=\"{link.Data.Title}\" class=\"govuk-link\"");

            // if the text content of the link is wrapped with "[]", then we make the link open in a new tab
            string? firstTextValue = link.Content.OfType<Text>().FirstOrDefault()?.Value;
            if (firstTextValue != null && firstTextValue.StartsWith('[') && firstTextValue.EndsWith(']'))
                sb.Append(" rel=\"noreferrer noopener\" target=\"_blank\"");

            sb.Append('>');

            // this common code could go in a base class
            foreach (var subContent in link.Content)
            {
                var renderer = _rendererCollection.GetRendererForContent(subContent);
                sb.Append(renderer.Render(subContent));
            }

            sb.Append("</a>");

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
