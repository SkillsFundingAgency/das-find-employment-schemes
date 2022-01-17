using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class PagesControllerTests
    {
        [Theory]
        [InlineData("cookies")]
        [InlineData("accessibility-statement")]
        public void PagesController_Page(string pageUrl)
        {
            ILogger<PagesController> logger = A.Fake<ILogger<PagesController>>();
            IContentService contentService = A.Fake<IContentService>();
            IContent content = A.Fake<IContent>();
            A.CallTo(() => contentService.Content).Returns(content);
            A.CallTo(() => content.Pages).Returns(new[]
            {
                new Page("", "cookies", null!),
                new Page("", "accessibility-statement", null!)
            });
            PagesController controller = new PagesController(logger, contentService);

            IActionResult result = controller.Page(pageUrl);
            Assert.True(result is ViewResult);
        }

        [Theory]
        [InlineData("doesnt-exist")]
        [InlineData("this-one-neither")]
        public void PagesController_PageNotFound(string pageUrl)
        {
            ILogger<PagesController> logger = A.Fake<ILogger<PagesController>>();
            IContentService contentService = A.Fake<IContentService>();
            PagesController controller = new PagesController(logger, contentService);

            IActionResult result = controller.Page(pageUrl);
            Assert.True(result is NotFoundResult);
        }
    }
}
