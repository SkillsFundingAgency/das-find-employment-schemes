using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class CaseStudiesControllerTests
    {
        public ICaseStudyPageService CaseStudyPageService { get; set; }
        public CaseStudiesController CaseStudiesController { get; set; }

        public CaseStudiesControllerTests()
        {
            CaseStudyPageService = A.Fake<ICaseStudyPageService>();

            CaseStudiesController = new CaseStudiesController(CaseStudyPageService);
        }

        [Fact]
        public void CaseStudyPage_KnownPageUrlReturnsViewResultWithDefaultViewTest()
        {
            const string pageUrl = "pageUrl";

            A.CallTo(() => CaseStudyPageService.GetCaseStudyPageModel(pageUrl))
                .Returns(new CaseStudyPageModel(new CaseStudyPage("title", "url", null!, null!)));

            // act
            IActionResult result = CaseStudiesController.CaseStudyPage(pageUrl);

            Assert.IsNotType<NotFoundResult>(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void CaseStudyPage_UnknownPageUrlReturnsNotFoundTest()
        {
            const string pageUrl = "unknown-page";

            A.CallTo(() => CaseStudyPageService.GetCaseStudyPageModel(pageUrl))
                .Returns(null);

            // act
            IActionResult result = CaseStudiesController.CaseStudyPage(pageUrl);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CaseStudyPagePreview_KnownPageUrlReturnsViewResultWithCorrectViewNameTest()
        {
            const string pageUrl = "pageUrl";

            A.CallTo(() => CaseStudyPageService.GetCaseStudyPageModelPreview(pageUrl))
                .Returns(new CaseStudyPageModel(new CaseStudyPage("title", "url", null!, null!)));

            // act
            IActionResult result = await CaseStudiesController.CaseStudyPagePreview(pageUrl);

            Assert.IsNotType<NotFoundResult>(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("CaseStudyPage", viewResult.ViewName);
        }

        [Fact]
        public async Task CaseStudyPagePreview_UnknownPageUrlReturnsNotFoundTest()
        {
            const string pageUrl = "unknown-page";

            CaseStudyPageModel nullCaseStudyPageModel = null;
            A.CallTo(() => CaseStudyPageService.GetCaseStudyPageModelPreview(pageUrl))
                .Returns(nullCaseStudyPageModel);

            // act
            IActionResult result = await CaseStudiesController.CaseStudyPagePreview(pageUrl);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
