
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;


namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class PagesControllerTests
    {
        [Theory]
        [ClassData(typeof(PagesControllerTestData))]
        public void PagesController_Page(Page expectedPage, string pageUrl)
        {
            ILogger<PagesController> logger = A.Fake<ILogger<PagesController>>();
            PagesController controller = A.Fake<PagesController>(x => x.WithArgumentsForConstructor(() => new PagesController(logger)));

            IActionResult result = controller.Page(pageUrl);
            Assert.True(result is ViewResult);
            ViewResult vr = (ViewResult)result;
            Assert.True(!(vr.Model is null) && vr.Model is Page);
            Page page = (Page)vr.Model;
            Assert.True(page.Title == expectedPage.Title);
        }
    }
    public class PagesControllerTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { SchemesContent.Pages.FirstOrDefault(p => p.Url == "accessibility-statement"), "accessibility-statement" };
            yield return new object[] { SchemesContent.Pages.FirstOrDefault(p => p.Url == "cookies"), "cookies" };
            yield return new object[] { SchemesContent.Pages.FirstOrDefault(p => p.Url == "privacy-notice"), "privacy-notice" };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
