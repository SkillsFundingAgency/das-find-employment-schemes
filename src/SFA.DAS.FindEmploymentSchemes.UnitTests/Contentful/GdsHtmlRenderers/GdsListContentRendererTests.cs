using Contentful.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.GdsHtmlRenderers
{
    // https://github.com/contentful/contentful.net/blob/master/Contentful.Core.Tests/Models/Rendering/HtmlRenderTests.cs
    public class GdsListContentRendererTests
    {
        [Fact]
        public async Task ToHtml_GdsListTests()
        {
            //Arrange
            var expectedResult = "<ul class=\"govuk-list govuk-list--bullet\"><li>testing</li></ul>";
            var renderer = ContentService.CreateHtmlRenderer();
            var doc = new Document();
            var list = new List();
            var listItem = new ListItem();
            var paragraph = new Paragraph();
            var text = new Text();
            text.Value = "testing";
            paragraph.Content = new List<IContent> { text };
            listItem.Content = new List<IContent> { paragraph };
            list.Content = new List<IContent> { listItem };
            doc.Content = new List<IContent> { list };

            //Act
            var result = await renderer.ToHtml(doc);

            //Assert
            Assert.Equal(expectedResult, result);
        }

    }
}
