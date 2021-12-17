using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Models
{
    public class SchemeDetailsModelTests
    {
        [Fact]
        public void Constructor()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IHtmlContent),
                    typeof(HtmlString)));

            var schemes = fixture.CreateMany<Scheme>(5).ToArray();

            const int selectedScheme = 2;

            var schemeUrl = schemes[selectedScheme].Url;
            var expectedOtherSchemes = schemes.Take(selectedScheme).Concat(schemes.Skip(selectedScheme+1));

            //Act
            var schemeDetailsModel = new SchemeDetailsModel(schemeUrl, schemes);

            Assert.Equal(schemes[2], schemeDetailsModel.Scheme);
            Assert.Equal(expectedOtherSchemes, schemeDetailsModel.OtherSchemes);
        }
    }
}