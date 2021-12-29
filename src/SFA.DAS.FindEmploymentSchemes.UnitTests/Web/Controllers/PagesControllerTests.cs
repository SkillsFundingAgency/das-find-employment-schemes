
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class PagesControllerTests
    {
        [Theory]
        [ClassData(typeof(PagesControllerTestData))]
        public void PagesController_Page(string expectedTitle, string pageUrl)
        {
            ILogger<PagesController> logger = A.Fake<ILogger<PagesController>>();
            PagesController controller = new PagesController(logger);
            Page expectedPage = A.Fake<Page>(x => x.WithArgumentsForConstructor(() => new Page(expectedTitle, pageUrl, null)));

            IActionResult result = controller.Page(pageUrl);
            Assert.True(result is ViewResult);
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
