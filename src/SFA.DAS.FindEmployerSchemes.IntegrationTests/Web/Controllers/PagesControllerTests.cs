using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;

namespace SFA.DAS.FindEmploymentSchemes.IntegrationTests.Web.Controllers
{
    public class PagesControllerTests
    {
        private readonly IServiceProvider _services = Program.GetServices();
        private ILogger<PagesController> _logger;
        private PagesController _controller;

        [Theory]
        [ClassData(typeof(PagesControllerTestData))]
        public void PagesController_Page(Page expectedPage, string pageUrl)
        {
            _logger = _services.GetRequiredService<ILogger<PagesController>>();
            Assert.NotNull(_logger);
            _controller = new PagesController(_logger);
            Assert.NotNull(_controller);

            IActionResult result = _controller.Page(pageUrl);
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
