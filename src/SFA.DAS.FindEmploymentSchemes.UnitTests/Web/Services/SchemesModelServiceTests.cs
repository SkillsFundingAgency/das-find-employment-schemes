using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using FakeItEasy;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;
using IContent2 = Contentful.Core.Models.IContent;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class SchemesModelServiceTests
    {
        public Fixture Fixture { get; }
        public IEnumerable<Page> NotHomepages { get; set; }
        public IContent Content { get; set; }
        public IContent PreviewContent { get; set; }
        public IContentService ContentService { get; set; }
        public SchemesModelService SchemesModelService { get; set; }

        public const string HomePagePreamblePrimary = "expectedPrimaryPreamble";

        public const string HomePagePreambleSecondary = "expectedSecondaryPreamble";

        private readonly ILogger<SchemesModelService> _logger;

        public SchemesModelServiceTests()
        {
            Fixture = new Fixture();

            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            Fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IContent),
                    typeof(Paragraph)));

            Fixture.Customizations.Add(
            new TypeRelay(
            typeof(IFieldValidator),
            typeof(Asset)));

            Fixture.Customizations.Add(
            new TypeRelay(
                typeof(IContent2),
                typeof(Document)));

            ContentService = A.Fake<IContentService>();
            (Content, NotHomepages) = CreateContent();
            (PreviewContent, _) = CreateContent();

            A.CallTo(() => ContentService.Content)
                .Returns(Content);

            A.CallTo(() => ContentService.UpdatePreview())
                .Returns(PreviewContent);

            _logger = A.Fake<ILogger<SchemesModelService>>();

            SchemesModelService = new SchemesModelService(_logger, ContentService);
        }

        private (IContent, IEnumerable<Page>) CreateContent()
        {
            var content = A.Fake<IContent>();

            var notHomepages = Fixture.CreateMany<Page>(3);

            var homePage = new Page(
                
                "", 
                
                "home", 
                
                null, 
                
                new InterimPreamble(),

                new InterimBreadcrumbs() { InterimBreadcrumbTitle = "InterimBreadcrumbTitle" }

            );

            var pages = notHomepages.Concat(new[] { homePage }).ToArray();

            A.CallTo(() => content.Pages)
                .Returns(pages);

            var schemes = Fixture.CreateMany<Scheme>(3).ToArray();

            A.CallTo(() => content.Schemes)
                .Returns(schemes);

            return (content, notHomepages);
        }

        [Fact]
        public void ContentServiceContentUpdated_UpdatesHomeModel()
        {
            const string updatedPreamble = "updatedPreamble";

            SchemesModelService = new SchemesModelService(_logger, ContentService);

            var notHomepages = Fixture.CreateMany<Page>(3);
            var homePage = new Page(string.Empty, "home", new HtmlString(updatedPreamble), new InterimPreamble() { PreambleTitle = "PreambleTitle1" }, new InterimBreadcrumbs() { InterimBreadcrumbTitle = "InterimBreadcrumbTitle1" });

            var pages = notHomepages.Concat(new[] { homePage }).ToArray();

            A.CallTo(() => Content.Pages)
                .Returns(pages);

            // Act
            ContentService.ContentUpdated += Raise.WithEmpty();

            Assert.Equal("PreambleTitle1", SchemesModelService.HomeModel.InterimPreamble.PreambleTitle);

            Assert.Equal("InterimBreadcrumbTitle1", SchemesModelService.HomeModel.InterimPageBreadcrumbs.InterimBreadcrumbTitle);

        }

        [Fact]
        public async Task CreateHomeModelPreview_IsPreviewIsTrueTest()
        {
            // act
            var model = await SchemesModelService.CreateHomeModelPreview();

            Assert.True(model.Preview.IsPreview);
        }

        [Fact]
        public async Task CreateHomeModelPreview_PreambleNull_PreviewErrorTest()
        {
            var homePage = new Page("", "home", null);

            var pages = NotHomepages.Concat(new[] { homePage }).ToArray();

            A.CallTo(() => PreviewContent.Pages)
                .Returns(pages);

            // act
            var model = await SchemesModelService.CreateHomeModelPreview();

            Assert.Collection(model.Preview.PreviewErrors,
                e => Assert.Equal("Preamble must not be blank", e.Value));
        }

        [Fact]
        public void GetSchemeDetailsModel_KnownSchemeUrl_ReturnsModelTest()
        {
            // act
            var model = SchemesModelService.GetSchemeDetailsModel(Content.Schemes.First().Url);

            Assert.NotNull(model);
        }

        [Fact]
        public void GetSchemeDetailsModel_UnknownSchemeUrl_ReturnsModelTest()
        {
            string unknownUrl = "unknownUrl";

            // act
            var model = SchemesModelService.GetSchemeDetailsModel(unknownUrl);

            Assert.Null(model);
        }

        [Fact]
        public async Task GetSchemeDetailsModelPreview__IsPreviewIsTrueTest()
        {
            // act
            var model = await SchemesModelService.GetSchemeDetailsModelPreview(PreviewContent.Schemes.First().Url);

            Assert.True(model.Preview.IsPreview);
        }

        [Theory]
        [ClassData(typeof(PreviewErrorTestData))]
        public async Task GetSchemeDetailsModelPreview_SingleMissingMandatoryField_PreviewErrorTest(string expected, Scheme scheme)
        {
            var schemes = new[] { scheme };

            A.CallTo(() => PreviewContent.Schemes)
                .Returns(schemes);

            // act
            var model = await SchemesModelService.GetSchemeDetailsModelPreview(PreviewContent.Schemes.First().Url);

            Assert.Collection(model.Preview.PreviewErrors, e => Assert.Equal(expected, e.Value));

        }

        [Fact]
        public void CreateComparisonResultsModel_IsPreviewIsFalseTest()
        {
            var firstTwoSchemeIds = Content.Schemes.Take(2).Select(s => s.HtmlId).ToArray();

            // act
            var model = SchemesModelService.CreateComparisonResultsModel(firstTwoSchemeIds, new SchemeFilterModel());

            Assert.False(model.Preview.IsPreview);
        }

        [Fact]
        public void CreateComparisonResultsModel_ModelTest()
        {
            var firstTwoSchemeIds = Content.Schemes.Take(2).Select(s => s.HtmlId).ToArray();

            // act
            var model = SchemesModelService.CreateComparisonResultsModel(firstTwoSchemeIds, new SchemeFilterModel());

            Assert.Collection(model.Schemes,
                s => Assert.Equal(firstTwoSchemeIds[0], s.HtmlId),
                s => Assert.Equal(firstTwoSchemeIds[1], s.HtmlId));
        }

        [Fact]
        public async Task CreateComparisonResultsModelPreview_IsPreviewIsTrueTest()
        {
            var firstTwoSchemeIds = Content.Schemes.Take(2).Select(s => s.HtmlId).ToArray();

            // act
            var model = await SchemesModelService.CreateComparisonResultsModelPreview(firstTwoSchemeIds, new SchemeFilterModel());

            Assert.True(model.Preview.IsPreview);
        }

        [Fact]
        public async Task CreateComparisonResultsModelPreview_ModelTest()
        {
            var firstTwoSchemeIds = PreviewContent.Schemes.Take(2).Select(s => s.HtmlId).ToArray();

            // act
            var model = await SchemesModelService.CreateComparisonResultsModelPreview(firstTwoSchemeIds, new SchemeFilterModel());

            Assert.Collection(model.Schemes,
                s => Assert.Equal(firstTwoSchemeIds[0], s.HtmlId),
                s => Assert.Equal(firstTwoSchemeIds[1], s.HtmlId));
        }

        public class PreviewErrorTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "Name must not be blank", new Scheme(null, "shortname", "visitSchemeInformation", new HtmlString("shortDescription"), "ShortBenefitsHeading", "ShortCostHeading", "ShortTimeHeading", new HtmlString("shortCost"), new HtmlString("shortBenefits"), new HtmlString("shortTime"), "comparisonRecruitOrTrain", "comparisonAgeCriteria", "comparisonCost", "comparisonDuration", "url", 0, new List<SchemeFilterAspect>(), Enumerable.Empty<string>(), Enumerable.Empty<CaseStudy>(), new HtmlString("caseStudiesPreamble"), new HtmlString("detailsPageOverride")) };
                yield return new object[] { "Short description must not be blank", new Scheme("name", "shortname", "visitSchemeInformation", null, "ShortBenefitsHeading", "ShortCostHeading", "ShortTimeHeading", new HtmlString("shortCost"), new HtmlString("shortBenefits"), new HtmlString("shortTime"), "comparisonRecruitOrTrain", "comparisonAgeCriteria", "comparisonCost", "comparisonDuration", "url", 0, new List<SchemeFilterAspect>(), Enumerable.Empty<string>(), Enumerable.Empty<CaseStudy>(), new HtmlString("caseStudiesPreamble"), new HtmlString("detailsPageOverride")) };
                yield return new object[] { "Short cost must not be blank", new Scheme("name", "shortname", "visitSchemeInformation", new HtmlString("shortDescription"), "ShortBenefitsHeading", "ShortCostHeading", "ShortTimeHeading", null, new HtmlString("shortBenefits"), new HtmlString("shortTime"), "comparisonRecruitOrTrain", "comparisonAgeCriteria", "comparisonCost", "comparisonDuration", "url", 0, new List<SchemeFilterAspect>(), Enumerable.Empty<string>(), Enumerable.Empty<CaseStudy>(), new HtmlString("caseStudiesPreamble"), new HtmlString("detailsPageOverride")) };
                yield return new object[] { "Short benefits must not be blank", new Scheme("name", "shortname", "visitSchemeInformation", new HtmlString("shortDescription"), "ShortBenefitsHeading", "ShortCostHeading", "ShortTimeHeading", new HtmlString("shortCost"), null, new HtmlString("shortTime"), "comparisonRecruitOrTrain", "comparisonAgeCriteria", "comparisonCost", "comparisonDuration", "url", 0, new List<SchemeFilterAspect>(), Enumerable.Empty<string>(), Enumerable.Empty<CaseStudy>(), new HtmlString("caseStudiesPreamble"), new HtmlString("detailsPageOverride")) };
                yield return new object[] { "Short time must not be blank", new Scheme("name", "shortname", "visitSchemeInformation", new HtmlString("shortDescription"), "ShortBenefitsHeading", "ShortCostHeading", "ShortTimeHeading", new HtmlString("shortCost"), new HtmlString("shortBenefits"), null, "comparisonRecruitOrTrain", "comparisonAgeCriteria", "comparisonCost", "comparisonDuration", "url", 0, new List<SchemeFilterAspect>(), Enumerable.Empty<string>(), Enumerable.Empty<CaseStudy>(), new HtmlString("caseStudiesPreamble"), new HtmlString("detailsPageOverride")) };
                yield return new object[] { "If there is no details page override and no sub schemes, the offer header must not be blank", new Scheme("name", "shortname", "visitSchemeInformation", new HtmlString("shortDescription"), "ShortBenefitsHeading", "ShortCostHeading", "ShortTimeHeading", new HtmlString("shortCost"), new HtmlString("shortBenefits"), new HtmlString("shortTime"), "comparisonRecruitOrTrain", "comparisonAgeCriteria", "comparisonCost", "comparisonDuration", "url", 0, new List<SchemeFilterAspect>(), Enumerable.Empty<string>(), Enumerable.Empty<CaseStudy>(), new HtmlString("caseStudiesPreamble"), null, null) };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        //todo: test to ensure content from UpdatePreview is used for preview gets
    }
}