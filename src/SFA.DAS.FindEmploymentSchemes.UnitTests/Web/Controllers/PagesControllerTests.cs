using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Services;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class PagesControllerTests
    {
        public IPageService PageService { get; set; }
        public ILogger<PagesController> Logger { get; set; }
        public PagesController PagesController { get; set; }
        public IContentService ContentService { get; set; }

        public PagesControllerTests()
        {
            Logger = A.Fake<ILogger<PagesController>>();
            PageService = A.Fake<IPageService>();
            ContentService = A.Fake<IContentService>();

            PagesController = new PagesController(Logger, ContentService, PageService);
        }

        [Fact]
        public void Page_KnownPageUrlReturnsViewResultWithDefaultViewTest()
        {
            const string pageUrl = "pageUrl";

            A.CallTo(() => PageService.Page(pageUrl, A<IContent>._))
                .Returns((null, new Page("title", "url", null)));

            IActionResult result = PagesController.Page(pageUrl);
            Assert.False(result is NotFoundResult);
            Assert.True(result is ViewResult);
            var viewResult = result as ViewResult;
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void Page_NonDefaultViewNameTest()
        {
            const string pageUrl = "pageUrl";
            const string nonDefaultViewName = "nonDefaultViewName";

            A.CallTo(() => PageService.Page(pageUrl, A<IContent>._))
                .Returns((nonDefaultViewName, new Page("title", "url", null)));

            IActionResult result = PagesController.Page(pageUrl);
            Assert.True(result is ViewResult);
            Assert.False(result is NotFoundResult);
            var viewResult = result as ViewResult;
            Assert.Equal(nonDefaultViewName, viewResult.ViewName);
        }

        [Fact]
        public void Page_UnknownPageUrlReturnsNotFoundTest()
        {
            const string pageUrl = "unknown-page";

            A.CallTo(() => PageService.Page(pageUrl, A<IContent>._))
                .Returns((null, null));

            IActionResult result = PagesController.Page(pageUrl);

            Assert.True(result is NotFoundResult);
        }
    }
}
