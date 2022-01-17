using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class PagesControllerTests
    {
        [Theory]
        [ClassData(typeof(PagesControllerTestData))]
        public void PagesController_Page(string expectedTitle, string pageUrl)
        {
            ILogger<PagesController> logger = A.Fake<ILogger<PagesController>>();
            IContentService contentService = A.Fake<IContentService>();
            PagesController controller = new PagesController(logger, contentService);
            Page expectedPage = A.Fake<Page>(x => x.WithArgumentsForConstructor(() => new Page(expectedTitle, pageUrl, null)));

            IActionResult result = controller.Page(pageUrl);
            Assert.True(result is ViewResult);
        }

        [Theory]
        [InlineData(null, "doesnt-exist")]
        [InlineData(null, "this-one-neither")]
        public void PagesController_PageNotFound(string expectedTitle, string pageUrl)
        {
            ILogger<PagesController> logger = A.Fake<ILogger<PagesController>>();
            IContentService contentService = A.Fake<IContentService>();
            PagesController controller = new PagesController(logger, contentService);
            Page expectedPage = A.Fake<Page>(x => x.WithArgumentsForConstructor(() => new Page(expectedTitle, pageUrl, null)));

            IActionResult result = controller.Page(pageUrl);
            Assert.True(result is NotFoundResult);
        }
    }

    public class PagesControllerTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "accessibility-statement-title", "accessibility-statement" };
            yield return new object[] { "cookies-title", "cookies" };
            yield return new object[] { "privacy-notice-title", "privacy-notice" };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
