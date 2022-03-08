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
            (document, expectedBenefits) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.Benefits = document;
            (document, expectedCaseStudiesPreamble) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.CaseStudies = document;
            (document, expectedCost) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.Cost = document;
            (document, expectedDescription) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.Description = document;
            (document, expectedDetailsPageOverride) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.DetailsPageOverride = document;
            (document, expectedOffer) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.Offer = document;
            (document, expectedResponsibility) = SampleDocumentAndExpectedContent(++differentiator);
            scheme.Responsibility = document;
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
            Assert.Equal(expectedBenefits.Value, actualScheme.Benefits.Value);
            Assert.Equal(expectedCaseStudiesPreamble.Value, actualScheme.CaseStudiesPreamble.Value);
            Assert.Equal(expectedCost.Value, actualScheme.Cost.Value);
            Assert.Equal(expectedDescription.Value, actualScheme.Description.Value);
            Assert.Equal(expectedDetailsPageOverride.Value, actualScheme.DetailsPageOverride.Value);
            Assert.Equal(expectedOffer.Value, actualScheme.Offer.Value);
            Assert.Equal(expectedResponsibility.Value, actualScheme.Responsibility.Value);
            Assert.Equal(expectedShortBenefits.Value, actualScheme.ShortBenefits.Value);
            Assert.Equal(expectedShortCost.Value, actualScheme.ShortCost.Value);
            Assert.Equal(expectedShortDescription.Value, actualScheme.ShortDescription.Value);
            Assert.Equal(expectedShortTime.Value, actualScheme.ShortTime.Value);
            //todo: enumerable fields
        }

        [Fact]
        public async Task Update_SchemesInDescendingSizeOrderTests()
        {
            var schemes = Fixture.CreateMany<Scheme>(3).ToArray();
            schemes[0].Size = 300;
            schemes[1].Size = 100;
            schemes[2].Size = 200;
            ContentfulCollection.Items = schemes;

            var schemesResult = await SchemeService.GetAll(ContentfulClient);

            var actualSchemes = schemesResult.ToArray();
            Assert.Equal(300, actualSchemes[0].Size);
            Assert.Equal(200, actualSchemes[1].Size);
            Assert.Equal(100, actualSchemes[2].Size);
        }
    }
}
