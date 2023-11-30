using System;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Model
{
    public class SchemeTests
    {
        [Fact]
        public void Ctor_ThrowsArgumentExceptionWhenUrlDoesntSanitizeToValidHtmlIdTest()
        {
            const string urlThatProducesInvalidId = "---";

            var htmlString = new HtmlString("");
            Assert.Throws<ArgumentException>(() => new Scheme("", "", "", htmlString, htmlString, htmlString, htmlString, string.Empty, string.Empty, string.Empty, string.Empty,
                urlThatProducesInvalidId, 0, new[] {""} ));
        }
    }
}
