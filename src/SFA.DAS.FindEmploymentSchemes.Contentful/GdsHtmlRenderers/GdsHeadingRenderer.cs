using System;
using System.Text;
using System.Threading.Tasks;
using Contentful.Core.Models;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.GdsHtmlRenderers
{
    /// <summary>
    /// A renderer for a heading.
    /// </summary>
    public class GdsHeadingRenderer : IContentRenderer
    {
        private readonly ContentRendererCollection _rendererCollection;

        /// <summary>
        /// Initializes a new HeadingRenderer.
        /// </summary>
        /// <param name="rendererCollection">The collection of renderer to use for sub-content.</param>
        public GdsHeadingRenderer(ContentRendererCollection rendererCollection)
        {
            _rendererCollection = rendererCollection;
        }

        public Task<string> RenderAsync(IContent content)
        {
            return Task.FromResult(Render(content));
        }

        /// <summary>
        /// The order of this renderer in the collection.
        /// </summary>
        public int Order { get; set; } = 50;

        /// <summary>
        /// Whether or not this renderer supports the provided content.
        /// </summary>
        /// <param name="content">The content to evaluate.</param>
        /// <returns>Returns true if the content is a heading, otherwise false.</returns>
        public bool SupportsContent(IContent content)
        {
            return content is Heading1 || content is Heading2 || content is Heading3 || content is Heading4;
        }

        /// <summary>
        /// Renders the content to a GDS compliant html h-tag.
        /// </summary>
        /// <param name="content">The content to render.</param>
        /// <returns>The h-tag as a string.</returns>
        public string Render(IContent content)
        {
            int headingSize;
            string gdsHeadingClassSize;

            switch (content)
            {
                case Heading1 _:
                    gdsHeadingClassSize = "xl";
                    headingSize = 1;
                    break;
                case Heading2 _:
                    gdsHeadingClassSize = "l";
                    headingSize = 2;
                    break;
                case Heading3 _:
                    gdsHeadingClassSize = "m";
                    headingSize = 3;
                    break;
                case Heading4 _:
                    gdsHeadingClassSize = "s";
                    headingSize = 3;
                    break;
                default:
                    throw new ArgumentException("Only H1-H4 are supported", nameof(content));
            }

            var heading = content as IHeading;

            var sb = new StringBuilder();
            sb.Append($"<h{headingSize} class=\"govuk-heading-{gdsHeadingClassSize}\">");

            // we assume HeadingN implements IHeading
            foreach (var subContent in heading!.Content)
            {
                var renderer = _rendererCollection.GetRendererForContent(subContent);
                sb.Append(renderer.Render(subContent));
            }

            sb.Append($"</h{headingSize}>");
            return sb.ToString();
        }
    }
}
