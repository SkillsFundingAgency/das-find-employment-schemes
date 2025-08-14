using System;
using System.Collections;
using System.Linq;
using Contentful.Core.Models;
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
            Assert.Throws<ArgumentException>(() => new Scheme(
                string.Empty, string.Empty, string.Empty,
                htmlString, string.Empty, string.Empty, string.Empty, htmlString, htmlString,
                htmlString, string.Empty, string.Empty, string.Empty, string.Empty, urlThatProducesInvalidId, 
                0, new System.Collections.Generic.List<SchemeFilterAspect>(), Enumerable.Empty<string>()));
        }
    }
}
