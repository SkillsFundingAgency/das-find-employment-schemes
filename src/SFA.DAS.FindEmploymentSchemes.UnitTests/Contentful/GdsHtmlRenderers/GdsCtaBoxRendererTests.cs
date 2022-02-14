using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.GdsHtmlRenderers
{
    public class GdsCtaBoxRendererTests
    {
        [Fact]
        public async Task ToHtml_GdsGdsCtaBoxRendererTests()
        {
            var renderer = ContentService.CreateHtmlRenderer();
            var doc = new Document
            {
                Content = new List<IContent>
                {
                    new Quote
                    {
                        Content = new List<IContent>
                        {
                            new Text
                            {
                                Value = "Gobble",
                            }
                        }
                    }
                }
            };

            var html = await renderer.ToHtml(doc);

            Assert.Equal("<div class=\"cx-cta-box\">Gobble</div>", html);
        }
    }
}
