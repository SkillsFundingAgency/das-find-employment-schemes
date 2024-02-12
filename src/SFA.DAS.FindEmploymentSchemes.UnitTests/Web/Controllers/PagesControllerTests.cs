using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class PagesControllerTests
    {
        public IContentService ContentService { get; set; }
        public IPageService PageService { get; set; }
        public PagesController PagesController { get; set; }

        public PagesControllerTests()
        {
            ContentService = A.Fake<IContentService>();
            PageService = A.Fake<IPageService>();

            A.CallTo(() => PageService.RedirectPreview(A<string>._))
                .Returns(default);

            PagesController = new PagesController(ContentService, PageService);

            PagesController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public void Page_KnownPageUrlReturnsViewResultWithViewNameFromModelTest()
        {
            const string pageUrl = "pageUrl";
            const string viewName = "viewName";

            A.CallTo(() => PageService.GetPageModel(pageUrl))
                .Returns(new PageModel(new Page("title", "url", null), [], viewName));

            // act
            IActionResult result = PagesController.Page(pageUrl);

            Assert.IsNotType<NotFoundResult>(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewName, viewResult.ViewName);
        }

        [Fact]
        public void Page_UnknownPageUrlReturnsNotFoundTest()
        {
            const string pageUrl = "unknown-page";

            A.CallTo(() => PageService.GetPageModel(pageUrl))
                .Returns(null);

            // act
            IActionResult result = PagesController.Page(pageUrl);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PagePreview_KnownPageUrlReturnsViewResultWithViewNameFromModelTest()
        {
            const string pageUrl = "pageUrl";
            const string viewName = "viewName";

            A.CallTo(() => PageService.GetPageModelPreview(pageUrl))
                .Returns(new PageModel(new Page("title", "url", null), [], viewName));

            // act
            IActionResult result = await PagesController.PagePreview(pageUrl);

            Assert.IsNotType<NotFoundResult>(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(viewName, viewResult.ViewName);
        }

        [Fact]
        public async Task PagePreview_UnknownPageUrlReturnsNotFoundTest()
        {
            const string pageUrl = "unknown-page";

            PageModel nullPageModel = null;
            A.CallTo(() => PageService.GetPageModelPreview(pageUrl))
                .Returns(nullPageModel);

            // act
            IActionResult result = await PagesController.PagePreview(pageUrl);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PagePreview_PageServiceRequestsRedirect_ReturnsRedirectWithCorrectUrlTest()
        {
            const string pageUrl = "redirect-from";
            const string redirectPageUrl = "redirect-to";

            A.CallTo(() => PageService.RedirectPreview(pageUrl))
                .Returns((redirectPageUrl, null));

            // act
            IActionResult result = await PagesController.PagePreview(pageUrl);

            var redirectToRouteResult = Assert.IsType<RedirectToRouteResult>(result);
            Assert.Equal(redirectPageUrl, redirectToRouteResult.RouteName);
        }

        [Fact]
        public async Task PagePreview_PageServiceRequestsRedirect_ReturnsRedirectWithCorrectRouteValuesTest()
        {
            const string pageUrl = "redirect-from";
            const string redirectPageUrl = "redirect-to";

            A.CallTo(() => PageService.RedirectPreview(pageUrl))
                .Returns((redirectPageUrl, new { pageUrl }));

            // act
            IActionResult result = await PagesController.PagePreview(pageUrl);

            var redirectToRouteResult = Assert.IsType<RedirectToRouteResult>(result);
            Assert.Equal(new RouteValueDictionary(new { pageUrl }), redirectToRouteResult.RouteValues);
        }

        [Fact]
        public void Cookies_ReturnsCookiesViewNameTest()
        {
            PagesController.ControllerContext.HttpContext.Request.Host = new HostString("localhost");

            // act
            IActionResult result = PagesController.Cookies("yes", "yes");

            Assert.IsNotType<NotFoundResult>(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Cookies", viewResult.ViewName);
        }
    }
}
