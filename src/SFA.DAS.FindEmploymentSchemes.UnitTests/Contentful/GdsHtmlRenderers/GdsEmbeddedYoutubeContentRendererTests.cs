
using Contentful.Core.Models;
//using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Contentful.ContentCodeGenerator;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;


namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.GdsHtmlRenderers
{
    public class GdsEmbeddedYoutubeContentRendererTests
    {
        [Fact]
        public async Task ToHtml_GdsEmbeddedYoutubeContentRendererTests()
        {
            string youtubeValue = "  <iframe>  abcdef...https://youtube.com/embed/something...vwxyz   </iframe>  ";
            //TODO: swap this over to ContentService.CreateHtmlRenderer() when merging later
            HtmlRenderer renderer = Program.CreateHtmlRenderer();
            Document doc = new Document
            {
                Content = new List<IContent>
                {
                    new Paragraph
                    {
                        Content = new List<IContent>
                        {
                            new Text
                            {
                                Value = youtubeValue,
                            }
                        }
                    }
                }
            };

            var html = await renderer.ToHtml(doc);
            Assert.Equal($"<p class=\"govuk-body\">{youtubeValue}</p>", html);
        }
    }
}
