﻿using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots.Base
{
    public class ContentRootServiceTests
    {
        [Theory]
        [InlineData("\"", "“")]
        [InlineData("\"", "”")]
        [InlineData("\"\"", "“”")]
        [InlineData("\r\n", "\r")]
        [InlineData("\r\n", "\r\n")]
        [InlineData("\r\n\r\n", "\r\r\n")]
        [InlineData("\r\nn", "\rn")]
        [InlineData("<br>", "<br>")]
        public void ToNormalisedHtmlString_Tests(string expectedHtmlStringValue, string html)
        {
            var result = ContentRootService.ToNormalisedHtmlString(html);

            Assert.Equal(expectedHtmlStringValue, result.Value);
        }
    }
}
