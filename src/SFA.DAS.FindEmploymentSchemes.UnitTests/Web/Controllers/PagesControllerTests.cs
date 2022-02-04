using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using NLog;

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

        [Theory]
        [InlineData("cookies")]
        [InlineData("accessibility-statement")]
        public void Page_KnownPageUrlReturnsRealViewResultTest(string pageUrl)
        {
            A.CallTo(() => PageService.Page(pageUrl, A<IContent>._))
                .Returns((null, new Page("title", "url", null)));

            IActionResult result = PagesController.Page(pageUrl);
            Assert.True(result is ViewResult);
            Assert.False(result is NotFoundResult);
        }

        [Theory]
        [InlineData("doesnt-exist")]
        [InlineData("this-one-neither")]
        public void Page_UnknownPageUrlReturnsNotFoundTest(string pageUrl)
        {
            A.CallTo(() => PageService.Page(pageUrl, A<IContent>._))
                .Returns((null, null));

            IActionResult result = PagesController.Page(pageUrl);
            Assert.True(result is NotFoundResult);
        }
    }
}
