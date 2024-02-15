using System.Linq;
using AutoFixture;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using System.Threading.Tasks;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{
    public class SchemeServiceTests : RootServiceTestBase<Scheme, SchemeService>
    {
        public SchemeService SchemeService { get; set; }

        public SchemeServiceTests()
        {
            SchemeService = new SchemeService(HtmlRenderer, Logger);
        }

        [Fact]
        public async Task GetAll_SameNumberOfSchemesTest()
        {
            const int numberOfSchemes = 3;

            ContentfulCollection.Items = Fixture.CreateMany<Scheme>(numberOfSchemes);

            var schemes = await SchemeService.GetAll(ContentfulClient);

            Assert.NotNull(schemes);
            Assert.Equal(numberOfSchemes, schemes.Count());
        }

        [Fact]
        public async Task GetAll_SchemeMappedTest()
        {
            ContentfulCollection.Items = Fixture.CreateMany<Scheme>(1);

            var scheme = ContentfulCollection.Items.First();
            int differentiator = 0;
            Document document;
            HtmlString expectedAdditionalFooter, expectedBenefits, expectedCaseStudiesPreamble, expectedCost, expectedDescription, expectedDetailsPageOverride, expectedOffer, expectedResponsibility,
                expectedShortBenefits, expectedShortCost, expectedShortDescription, expectedShortTime;

            (document, expectedAdditionalFooter) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.AdditionalFooter = document;
            (document, expectedCaseStudiesPreamble) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.CaseStudies = document;
            (document, expectedDetailsPageOverride) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.DetailsPageOverride = document;
            (document, expectedShortBenefits) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.ShortBenefits = document;
            (document, expectedShortCost) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.ShortCost = document;
            (document, expectedShortDescription) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.ShortDescription = document;
            (document, expectedShortTime) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.ShortTime = document;

            var schemes = await SchemeService.GetAll(ContentfulClient);

            var actualScheme = schemes.FirstOrDefault();
            Assert.NotNull(actualScheme);

            //todo: should really copy item before ContentService.Update() (or pick out fields), in case they get mutated
            // or even better add a test to check that they don't get mutated
            var expectedSourceScheme = ContentfulCollection.Items.First();
            Assert.Equal(expectedSourceScheme.Url, actualScheme.Url);
            Assert.Equal(expectedSourceScheme.Name, actualScheme.Name);
            Assert.Equal(expectedSourceScheme.OfferHeader, actualScheme.OfferHeader);
            Assert.Equal(expectedSourceScheme.Size, actualScheme.Size);
            Assert.Equal(expectedAdditionalFooter.Value, actualScheme.AdditionalFooter.Value);
            Assert.Equal(expectedCaseStudiesPreamble.Value, actualScheme.CaseStudiesPreamble.Value);
            Assert.Equal(expectedDetailsPageOverride.Value, actualScheme.DetailsPageOverride.Value);
            Assert.Equal(expectedShortBenefits.Value, actualScheme.ShortBenefits.Value);
            Assert.Equal(expectedShortCost.Value, actualScheme.ShortCost.Value);
            Assert.Equal(expectedShortDescription.Value, actualScheme.ShortDescription.Value);
            Assert.Equal(expectedShortTime.Value, actualScheme.ShortTime.Value);
            //todo: enumerable fields
        }

        [Fact]
        public async Task Update_SchemesInDescendingDefaultOrderTests()
        {
            var schemes = Fixture.CreateMany<Scheme>(3).ToArray();
            schemes[0].DefaultOrder = 1;
            schemes[1].DefaultOrder = 2;
            schemes[2].DefaultOrder = 3;
            ContentfulCollection.Items = schemes;

            var schemesResult = await SchemeService.GetAll(ContentfulClient);

            var actualSchemes = schemesResult.ToArray();
            Assert.Equal(1, actualSchemes[0].DefaultOrder);
            Assert.Equal(2, actualSchemes[1].DefaultOrder);
            Assert.Equal(3, actualSchemes[2].DefaultOrder);
        }
    }
}
