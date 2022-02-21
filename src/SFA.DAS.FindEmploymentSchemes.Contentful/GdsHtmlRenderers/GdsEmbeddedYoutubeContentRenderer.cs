using System.Linq;
using Contentful.Core.Models;
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
            if (!(content is Paragraph paragraph))
                return false;

            if (paragraph.Content.Count != 1 || !(paragraph.Content[0] is Text))
                return false;

            string text = ((Text)paragraph.Content[0]).Value.Trim();

            return text.StartsWith("<iframe") && text.EndsWith("</iframe>") &&
                   (text.Contains("youtube.com/embed/") || text.Contains("youtube-nocookie.com/embed/"));
        }

        //todo: this doesn't work

        /// <summary>
        /// Renders the content raw inside an html p-tag
        /// </summary>
        /// <param name="content">The content to render.</param>
        /// <returns>The p-tag as a string.</returns>
        public Task<string> RenderAsync(IContent content)
        {
            var paragraph = content as Paragraph;
            var sb = new StringBuilder();
            sb.Append("<p class=\"govuk-body\"><div class=\"app-video-container\">");

            var subContent = paragraph!.Content.FirstOrDefault() as Text;

            sb.Append(subContent!.Value.Replace("youtube.com/embed/", "youtube-nocookie.com/embed/", System.StringComparison.InvariantCultureIgnoreCase));

            sb.Append("</div></p>");
            return Task.FromResult(sb.ToString());
        }
    }
}