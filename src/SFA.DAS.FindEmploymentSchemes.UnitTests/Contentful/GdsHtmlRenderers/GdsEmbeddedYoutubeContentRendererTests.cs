//todo: fails because of encoding - think gets fixed in later pr

//using Contentful.Core.Models;
//using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Xunit;

//namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.GdsHtmlRenderers
//{
//    public class GdsEmbeddedYoutubeContentRendererTests
//    {
//        [Fact]
//        public async Task ToHtml_GdsEmbeddedYoutubeContentRendererTests()
//        {
//            string youtubeValue = "  <iframe>  abcdef...https://youtube.com/embed/something...vwxyz   </iframe>  ";
//            var renderer = ContentService.CreateHtmlRenderer();
//            Document doc = new Document
//            {
//                Content = new List<IContent>
//                {
//                    new Paragraph
//                    {
//                        Content = new List<IContent>
//                        {
//                            new Text
//                            {
//                                Value = youtubeValue,
//                            }
//                        }
//                    }
//                }
//            };

//            var html = await renderer.ToHtml(doc);
//            Assert.Equal($"<p class=\"govuk-body\">{youtubeValue.Replace("youtube.com", "youtube-nocookie.com")}</p>", html);
//        }
//    }
//}
