//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using FakeItEasy;
//using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
//using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
//using Xunit;
//using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
//using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
//using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
//using Microsoft.AspNetCore.Routing;
//using SFA.DAS.FindEmploymentSchemes.Web.Models;

//namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
//{
//    public class PagesControllerTests
//    {
//        public IContentService ContentService { get; set; }
//        public IPageService PageService { get; set; }
//        public PagesController PagesController { get; set; }

//        public PagesControllerTests()
//        {
//            ContentService = A.Fake<IContentService>();
//            PageService = A.Fake<IPageService>();

//            A.CallTo(() => PageService.RedirectPreview(A<string>._))
//                .Returns(default);

//            PagesController = new PagesController(ContentService, PageService);
//        }

//        [Fact]
//        public void Page_KnownPageUrlReturnsViewResultWithDefaultViewTest()
//        {
//            const string pageUrl = "pageUrl";

//            A.CallTo(() => PageService.GetPageModel(pageUrl))
//                .Returns(new PageModel(new Page("title", "url", null)));

//            // act
//            IActionResult result = PagesController.Page(pageUrl);

//            Assert.IsNotType<NotFoundResult>(result);
//            Assert.IsType<ViewResult>(result);
//            var viewResult = (ViewResult)result;
//            Assert.Null(viewResult.ViewName);
//        }

//        [Fact]
//        public void Page_NonDefaultViewNameTest()
//        {
//            const string pageUrl = "pageUrl";
//            const string nonDefaultViewName = "nonDefaultViewName";

//            A.CallTo(() => PageService.Page(pageUrl, A<IContent>._))
//                .Returns((nonDefaultViewName, new Page("title", "url", null)));

//            // act
//            IActionResult result = PagesController.Page(pageUrl);

//            Assert.IsNotType<NotFoundResult>(result);
//            Assert.IsType<ViewResult>(result);
//            var viewResult = (ViewResult)result;
//            Assert.Equal(nonDefaultViewName, viewResult.ViewName);
//        }

//        [Fact]
//        public void Page_UnknownPageUrlReturnsNotFoundTest()
//        {
//            const string pageUrl = "unknown-page";

//            A.CallTo(() => PageService.Page(pageUrl, A<IContent>._))
//                .Returns((null, null));

//            // act
//            IActionResult result = PagesController.Page(pageUrl);

//            Assert.IsType<NotFoundResult>(result);
//        }

//        [Fact]
//        public async Task PagePreview_UpdatesPreviewContentTest()
//        {
//            // act
//            await PagesController.PagePreview("");

//            A.CallTo(() => ContentService.UpdatePreview())
//                .MustHaveHappenedOnceExactly();
//        }

//        [Fact]
//        public async Task PagePreview_KnownPageUrlReturnsViewResultWithPageViewTest()
//        {
//            const string pageUrl = "pageUrl";

//            A.CallTo(() => PageService.Page(pageUrl, A<IContent>._))
//                .Returns((null, new Page("title", "url", null)));

//            // act
//            IActionResult result = await PagesController.PagePreview(pageUrl);

//            Assert.IsNotType<NotFoundResult>(result);
//            Assert.IsType<ViewResult>(result);
//            var viewResult = (ViewResult)result;
//            Assert.Equal("Page", viewResult.ViewName);
//        }

//        [Fact]
//        public async Task PagePreview_NonDefaultViewNameTest()
//        {
//            const string pageUrl = "pageUrl";
//            const string nonDefaultViewName = "nonDefaultViewName";

//            A.CallTo(() => PageService.Page(pageUrl, A<IContent>._))
//                .Returns((nonDefaultViewName, new Page("title", "url", null)));

//            // act
//            IActionResult result = await PagesController.PagePreview(pageUrl);

//            Assert.IsNotType<NotFoundResult>(result);
//            Assert.IsType<ViewResult>(result);
//            var viewResult = (ViewResult)result;
//            Assert.Equal(nonDefaultViewName, viewResult.ViewName);
//        }

//        [Fact]
//        public async Task PagePreview_UnknownPageUrlReturnsNotFoundTest()
//        {
//            const string pageUrl = "unknown-page";

//            A.CallTo(() => PageService.Page(pageUrl, A<IContent>._))
//                .Returns((null, null));

//            // act
//            IActionResult result = await PagesController.PagePreview(pageUrl);

//            Assert.IsType<NotFoundResult>(result);
//        }

//        [Fact]
//        public async Task PagePreview_PageServiceRequestsRedirect_ReturnsRedirectWithCorrectUrlTest()
//        {
//            const string pageUrl = "redirect-from";
//            const string redirectPageUrl = "redirect-to";

//            A.CallTo(() => PageService.RedirectPreview(pageUrl))
//                .Returns((redirectPageUrl, null));

//            // act
//            IActionResult result = await PagesController.PagePreview(pageUrl);

//            var redirectToRouteResult = Assert.IsType<RedirectToRouteResult>(result);
//            Assert.Equal(redirectPageUrl, redirectToRouteResult.RouteName);
//        }

//        [Fact]
//        public async Task PagePreview_PageServiceRequestsRedirect_ReturnsRedirectWithCorrectRouteValuesTest()
//        {
//            const string pageUrl = "redirect-from";
//            const string redirectPageUrl = "redirect-to";

//            A.CallTo(() => PageService.RedirectPreview(pageUrl))
//                .Returns((redirectPageUrl, new { pageUrl }));

//            // act
//            IActionResult result = await PagesController.PagePreview(pageUrl);

//            var redirectToRouteResult = Assert.IsType<RedirectToRouteResult>(result);
//            Assert.Equal(new RouteValueDictionary(new { pageUrl }), redirectToRouteResult.RouteValues);
//        }
//    }
//}
