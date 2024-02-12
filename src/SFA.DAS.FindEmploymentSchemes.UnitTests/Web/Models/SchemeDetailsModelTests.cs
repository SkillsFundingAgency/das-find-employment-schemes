using System.Linq;
using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Models
{
    public class SchemeDetailsModelTests
    {
        public Fixture Fixture { get; set; }
        public Scheme[] Schemes { get; set; }

        public SchemeDetailsModelTests()
        {
            Fixture = new Fixture();

            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            Fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IHtmlContent),
                    typeof(HtmlString)));

            Fixture.Customizations.Add(
            new TypeRelay(
                typeof(IContent),
                typeof(Document)));

            Schemes = Fixture.CreateMany<Scheme>(5).ToArray();
        }

        [Fact]
        public void Scheme_IsCorrectTest()
        {
            const int selectedScheme = 2;

            var schemeUrl = Schemes[selectedScheme].Url;

            //Act
            var schemeDetailsModel = new SchemeDetailsModel(schemeUrl, Schemes, []);

            Assert.Equal(Schemes[2], schemeDetailsModel.Scheme);
        }

        [Fact]
        public void Schemes_AreCorrectTest()
        {
            var fixture = new Fixture();

            var schemeUrl = Schemes[0].Url;

            //Act
            var schemeDetailsModel = new SchemeDetailsModel(schemeUrl, Schemes, []);

            Assert.Equal(Schemes, schemeDetailsModel.Schemes);
        }
    }
}