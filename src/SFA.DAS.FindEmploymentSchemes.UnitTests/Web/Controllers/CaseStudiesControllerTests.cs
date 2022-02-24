using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class CaseStudiesControllerTests
    {
        public ICaseStudyPageService CaseStudyPageService { get; set; }
        public IContentService ContentService { get; set; }
        public CaseStudiesController CaseStudiesController { get; set; }

        public CaseStudiesControllerTests()
        {
            CaseStudyPageService = A.Fake<ICaseStudyPageService>();
            ContentService = A.Fake<IContentService>();

            CaseStudiesController = new CaseStudiesController(CaseStudyPageService, ContentService);
        }

        [Fact]
        public void CaseStudyPage_KnownPageUrlReturnsViewResultWithDefaultViewTest()
        {
            const string pageUrl = "pageUrl";

            A.CallTo(() => CaseStudyPageService.CaseStudyPage(pageUrl, A<IContent>._))
                .Returns((null, new CaseStudyPage("title", "url", null!, null!)));

            // act
            IActionResult result = CaseStudiesController.CaseStudyPage(pageUrl);

            Assert.IsNotType<NotFoundResult>(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void CaseStudyPage_NonDefaultViewNameTest()
        {
            const string pageUrl = "pageUrl";
            const string nonDefaultViewName = "nonDefaultViewName";

            A.CallTo(() => CaseStudyPageService.CaseStudyPage(pageUrl, A<IContent>._))
                .Returns((nonDefaultViewName, new CaseStudyPage("title", "url", null!, null!)));

            // act
            IActionResult result = CaseStudiesController.CaseStudyPage(pageUrl);

            Assert.IsNotType<NotFoundResult>(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal(nonDefaultViewName, viewResult.ViewName);
        }

        [Fact]
        public void CaseStudyPage_UnknownPageUrlReturnsNotFoundTest()
        {
            const string pageUrl = "unknown-page";

            A.CallTo(() => CaseStudyPageService.CaseStudyPage(pageUrl, A<IContent>._))
                .Returns((null, null));

            // act
            IActionResult result = CaseStudiesController.CaseStudyPage(pageUrl);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CaseStudyPagePreview_UpdatesPreviewContentTest()
        {
            // act
            await CaseStudiesController.CaseStudyPagePreview("");

            A.CallTo(() => ContentService.UpdatePreview())
                .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CaseStudyPagePreview_KnownPageUrlReturnsViewResultWithPageViewTest()
        {
            const string pageUrl = "pageUrl";

            A.CallTo(() => CaseStudyPageService.CaseStudyPage(pageUrl, A<IContent>._))
                .Returns((null, new CaseStudyPage("title", "url", null!, null!)));

            // act
            IActionResult result = await CaseStudiesController.CaseStudyPagePreview(pageUrl);

            Assert.IsNotType<NotFoundResult>(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("CaseStudyPage", viewResult.ViewName);
        }

        [Fact]
        public async Task CaseStudyPagePreview_NonDefaultViewNameTest()
        {
            const string pageUrl = "pageUrl";
            const string nonDefaultViewName = "nonDefaultViewName";

            A.CallTo(() => CaseStudyPageService.CaseStudyPage(pageUrl, A<IContent>._))
                .Returns((nonDefaultViewName, new CaseStudyPage("title", "url", null!, null!)));

            // act
            IActionResult result = await CaseStudiesController.CaseStudyPagePreview(pageUrl);

            Assert.IsNotType<NotFoundResult>(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal(nonDefaultViewName, viewResult.ViewName);
        }

        [Fact]
        public async Task CaseStudyPagePreview_UnknownPageUrlReturnsNotFoundTest()
        {
            const string pageUrl = "unknown-page";

            A.CallTo(() => CaseStudyPageService.CaseStudyPage(pageUrl, A<IContent>._))
                .Returns((null, null));

            // act
            IActionResult result = await CaseStudiesController.CaseStudyPagePreview(pageUrl);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
