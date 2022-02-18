using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FakeItEasy;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services
{
    public class ContentServiceTests
    {
        public Fixture Fixture { get; }
        public Document Document { get; set; }
        public string ExpectedContent { get; set; }
        public IContentfulClientFactory ContentfulClientFactory { get; set; }
        public IContentfulClient ContentfulClient { get; set; }
        public IContentfulClient PreviewContentfulClient { get; set; }
        public HtmlRenderer HtmlRenderer { get; set; }
        public ILogger<ContentService> Logger { get; set; }
        public ContentfulCollection<Page> PagesCollection { get; set; }
        public ContentfulCollection<CaseStudyPage> CaseStudyPagesCollection { get; set; }
        public ContentfulCollection<Scheme> SchemesCollection { get; set; }
        public ContentfulCollection<Filter> FiltersCollection { get; set; }
        public ContentService ContentService { get; set; }
        public CompareLogic CompareLogic { get; set; }

        public ContentServiceTests()
        {
            Fixture = new Fixture();

            (Document, ExpectedContent) = SampleDocumentAndExpectedContent();

            Fixture.Inject(Document);

            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => Fixture.Behaviors.Remove(b));
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            Fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IContent),
                    typeof(Paragraph)));

            ContentfulClientFactory = A.Fake<IContentfulClientFactory>();
            ContentfulClient = A.Fake<IContentfulClient>();
            PreviewContentfulClient = A.Fake<IContentfulClient>();
            HtmlRenderer = A.Fake<HtmlRenderer>();
            Logger = A.Fake<ILogger<ContentService>>();

            A.CallTo(() => ContentfulClientFactory.ContentfulClient)
                .Returns(ContentfulClient);

            A.CallTo(() => ContentfulClientFactory.PreviewContentfulClient)
                .Returns(PreviewContentfulClient);

            PagesCollection = new ContentfulCollection<Page> { Items = Array.Empty<Page>() };
            CaseStudyPagesCollection = new ContentfulCollection<CaseStudyPage> { Items = Array.Empty<CaseStudyPage>() };
            SchemesCollection = new ContentfulCollection<Scheme> { Items = Array.Empty<Scheme>() };
            FiltersCollection = new ContentfulCollection<Filter> { Items = Array.Empty<Filter>() };

            SetupContentfulClientCalls(ContentfulClient);
            SetupContentfulClientCalls(PreviewContentfulClient);

            ContentService = new ContentService(ContentfulClientFactory, HtmlRenderer, Logger);

            CompareLogic = new CompareLogic();
        }

        private void SetupContentfulClientCalls(IContentfulClient contentfulClient)
        {
            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Page>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(PagesCollection);

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<CaseStudyPage>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(CaseStudyPagesCollection);

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Scheme>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(SchemesCollection);

            //A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Filter>>.Ignored, A<CancellationToken>.Ignored))
            //    .Returns(payFiltersCollection).Once()
            //    .Then
            //    .Returns(new ContentfulCollection<Filter>());

            // decouples us from the order of fetching filters
            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Filter>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(FiltersCollection);
        }

        private (Document, string) SampleDocumentAndExpectedContent(int differentiator = 0)
        {
            return (new Document
            {
                NodeType = "heading-2",
                Data = new GenericStructureData(),
                Content = new List<IContent>
                {
                    new Heading2
                    {
                        Content = new List<IContent> {new Text {Value = $"Gobble{differentiator}" } }
                    }
                }
            }, $"<h2>Gobble{differentiator}</h2>");
        }

        //[Fact]
        //public async Task Update_SchemesInDescendingSizeOrderTests()
        //{
        //    var schemes = Fixture.CreateMany<Scheme>(3).ToArray();
        //    schemes[0].Size = 300;
        //    schemes[1].Size = 100;
        //    schemes[2].Size = 200;
        //    SchemesCollection.Items = schemes;

        //    var content = await ContentService.Update();

        //    var actualSchemes = content.Schemes.ToArray();
        //    Assert.Equal(300, actualSchemes[0].Size);
        //    Assert.Equal(200, actualSchemes[1].Size);
        //    Assert.Equal(100, actualSchemes[2].Size);
        //}

        [Theory]
        [InlineData("pay--the-name", "the name")]
        [InlineData("pay--thename", "thename")]
        [InlineData("pay--the-name", "the-name")]
        //todo: stop double spaces, so code doesn't get confused with prefix/name separator?
        [InlineData("pay--the--name", "the  name")]
        [InlineData("pay--", "")]
        [InlineData("pay--1234567890-qwertyuiop-asdfghjkl-zxcvbnm", "1234567890 qwertyuiop asdfghjkl zxcvbnm")]
        public async Task Update_FilterIdTests(string expectedFilterAspectId, string filterName)
        {
            var filters = Fixture.CreateMany<Filter>(1).ToList();
            filters.First().Name = filterName;
            FiltersCollection.Items = filters;

            var content = await ContentService.Update();

            Assert.Equal(expectedFilterAspectId, content.PayFilter.Aspects.First().Id);
        }

        [Fact]
        public async Task Update_MissingContentfulClientThrowsExceptionTest()
        {
            A.CallTo(() => ContentfulClientFactory.ContentfulClient)
                .Returns(null);

            ContentService = new ContentService(ContentfulClientFactory, HtmlRenderer, Logger);

            await Assert.ThrowsAsync<ContentServiceException>(() => ContentService.Update());
        }

        [Fact]
        public async Task Update_ContentUpdatedEventIsRaisedTest()
        {
            bool eventWasRaised = false;
            ContentService.ContentUpdated += (sender, args) => { eventWasRaised = true; };

            var content = await ContentService.Update();

            Assert.True(eventWasRaised);
        }

        [Fact]
        public async Task Update_SameNumberOfPagesTest()
        {
            const int numberOfPages = 3;

            PagesCollection.Items = Fixture.CreateMany<Page>(numberOfPages);

            var content = await ContentService.Update();

            Assert.NotNull(content.Pages);
            Assert.Equal(numberOfPages, content.Pages.Count());
        }

        [Fact]
        public async Task Update_PageMappedTest()
        {
            const int numberOfPages = 1;

            PagesCollection.Items = Fixture.CreateMany<Page>(numberOfPages);

            var content = await ContentService.Update();

            var actualPage = content.Pages.FirstOrDefault();
            Assert.NotNull(actualPage);

            var expectedSourcePage = PagesCollection.Items.First();
            Assert.Equal(expectedSourcePage.Title, actualPage.Title);
            Assert.Equal(expectedSourcePage.Url, actualPage.Url);
            Assert.Equal(ExpectedContent, actualPage.Content.Value);
        }

        [Fact]
        public async Task Update_SameNumberOfCaseStudyPagesTest()
        {
            const int numberOfCaseStudyPages = 3;

            SchemesCollection.Items = Fixture.CreateMany<Scheme>(1);

            Fixture.Inject(SchemesCollection.Items.First());
            CaseStudyPagesCollection.Items = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages);

            var content = await ContentService.Update();

            Assert.NotNull(content.CaseStudyPages);
            Assert.Equal(numberOfCaseStudyPages, content.CaseStudyPages.Count());
        }

        [Fact]
        public async Task Update_CaseStudyPageMappedTest()
        {
            const int numberOfCaseStudyPages = 1;

            SchemesCollection.Items = Fixture.CreateMany<Scheme>(1);

            Fixture.Inject(SchemesCollection.Items.First());
            CaseStudyPagesCollection.Items = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages);

            var content = await ContentService.Update();

            var actualCaseStudyPage = content.CaseStudyPages.FirstOrDefault();
            Assert.NotNull(actualCaseStudyPage);

            var expectedSourceCaseStudyPage = CaseStudyPagesCollection.Items.First();
            Assert.Equal(expectedSourceCaseStudyPage.Title, actualCaseStudyPage.Title);
            Assert.Equal(expectedSourceCaseStudyPage.Url, actualCaseStudyPage.Url);
            Assert.Equal(SchemesCollection.Items.First().Url, actualCaseStudyPage.Scheme.Url);
            Assert.Equal(ExpectedContent, actualCaseStudyPage.Content.Value);
        }

        [Fact]
        public async Task Update_SameNumberOfSchemesTest()
        {
            const int numberOfSchemes = 3;

            SchemesCollection.Items = Fixture.CreateMany<Scheme>(numberOfSchemes);

            var content = await ContentService.Update();

            Assert.NotNull(content.Schemes);
            Assert.Equal(numberOfSchemes, content.Schemes.Count());
        }

        [Fact]
        public async Task Update_SchemeMappedTest()
        {
            SchemesCollection.Items = Fixture.CreateMany<Scheme>(1);

            var scheme = SchemesCollection.Items.First();
            int differentiator = 0;
            Document document;
            string expectedAdditionalFooter, expectedBenefits, expectedCaseStudiesPreamble, expectedCost, expectedDescription, expectedDetailsPageOverride, expectedOffer, expectedResponsibility,
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

            var content = await ContentService.Update();

            var actualScheme = content.Schemes.FirstOrDefault();
            Assert.NotNull(actualScheme);

            //todo: should really copy item before ContentService.Update() (or pick out fields), in case they get mutated
            // or even better add a test to check that they don't get mutated
            var expectedSourceScheme = SchemesCollection.Items.First();
            Assert.Equal(expectedSourceScheme.Url, actualScheme.Url);
            Assert.Equal(expectedSourceScheme.Name, actualScheme.Name);
            Assert.Equal(expectedSourceScheme.OfferHeader, actualScheme.OfferHeader);
            Assert.Equal(expectedSourceScheme.Size, actualScheme.Size);
            Assert.Equal(expectedAdditionalFooter, actualScheme.AdditionalFooter.Value);
            Assert.Equal(expectedBenefits, actualScheme.Benefits.Value);
            Assert.Equal(expectedCaseStudiesPreamble, actualScheme.CaseStudiesPreamble.Value);
            Assert.Equal(expectedCost, actualScheme.Cost.Value);
            Assert.Equal(expectedDescription, actualScheme.Description.Value);
            Assert.Equal(expectedDetailsPageOverride, actualScheme.DetailsPageOverride.Value);
            Assert.Equal(expectedOffer, actualScheme.Offer.Value);
            Assert.Equal(expectedResponsibility, actualScheme.Responsibility.Value);
            Assert.Equal(expectedShortBenefits, actualScheme.ShortBenefits.Value);
            Assert.Equal(expectedShortCost, actualScheme.ShortCost.Value);
            Assert.Equal(expectedShortDescription, actualScheme.ShortDescription.Value);
            Assert.Equal(expectedShortTime, actualScheme.ShortTime.Value);
            //todo: enumerable fields
        }

        [Fact]
        public async Task UpdatePreview_MissingContentfulClientThrowsExceptionTest()
        {
            A.CallTo(() => ContentfulClientFactory.PreviewContentfulClient)
                .Returns(null);

            ContentService = new ContentService(ContentfulClientFactory, HtmlRenderer, Logger);

            await Assert.ThrowsAsync<ContentServiceException>(() => ContentService.UpdatePreview());
        }

        [Fact]
        public async Task UpdatePreview_PreviewContentUpdatedEventIsRaisedTest()
        {
            bool eventWasRaised = false;
            ContentService.PreviewContentUpdated += (sender, args) => { eventWasRaised = true; };

            var content = await ContentService.UpdatePreview();

            Assert.True(eventWasRaised);
        }

        [Fact]
        public async Task UpdatePreview_SameNumberOfPagesTest()
        {
            const int numberOfPages = 3;

            PagesCollection.Items = Fixture.CreateMany<Page>(numberOfPages);

            var content = await ContentService.UpdatePreview();

            Assert.NotNull(content.Pages);
            Assert.Equal(numberOfPages, content.Pages.Count());
        }

        [Fact]
        public async Task UpdatePreview_PageMappedTest()
        {
            const int numberOfPages = 1;

            PagesCollection.Items = Fixture.CreateMany<Page>(numberOfPages);

            var content = await ContentService.UpdatePreview();

            var actualPage = content.Pages.FirstOrDefault();
            Assert.NotNull(actualPage);

            var expectedSourcePage = PagesCollection.Items.First();
            Assert.Equal(expectedSourcePage.Title, actualPage.Title);
            Assert.Equal(expectedSourcePage.Url, actualPage.Url);
            Assert.Equal(ExpectedContent, actualPage.Content.Value);
        }

        [Fact]
        public async Task UpdatePreview_SameNumberOfCaseStudyPagesTest()
        {
            const int numberOfCaseStudyPages = 3;

            SchemesCollection.Items = Fixture.CreateMany<Scheme>(1);

            Fixture.Inject(SchemesCollection.Items.First());
            CaseStudyPagesCollection.Items = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages);

            var content = await ContentService.UpdatePreview();

            Assert.NotNull(content.CaseStudyPages);
            Assert.Equal(numberOfCaseStudyPages, content.CaseStudyPages.Count());
        }

        [Fact]
        public async Task UpdatePreview_CaseStudyPageMappedTest()
        {
            const int numberOfCaseStudyPages = 1;

            SchemesCollection.Items = Fixture.CreateMany<Scheme>(1);

            Fixture.Inject(SchemesCollection.Items.First());
            CaseStudyPagesCollection.Items = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages);

            var content = await ContentService.UpdatePreview();

            var actualCaseStudyPage = content.CaseStudyPages.FirstOrDefault();
            Assert.NotNull(actualCaseStudyPage);

            var expectedSourceCaseStudyPage = CaseStudyPagesCollection.Items.First();
            Assert.Equal(expectedSourceCaseStudyPage.Title, actualCaseStudyPage.Title);
            Assert.Equal(expectedSourceCaseStudyPage.Url, actualCaseStudyPage.Url);
            Assert.Equal(SchemesCollection.Items.First().Url, actualCaseStudyPage.Scheme.Url);
            Assert.Equal(ExpectedContent, actualCaseStudyPage.Content.Value);
        }

        [Fact]
        public async Task UpdatePreview_SameNumberOfSchemesTest()
        {
            const int numberOfSchemes = 3;

            SchemesCollection.Items = Fixture.CreateMany<Scheme>(numberOfSchemes);

            var content = await ContentService.UpdatePreview();

            Assert.NotNull(content.Schemes);
            Assert.Equal(numberOfSchemes, content.Schemes.Count());
        }

        [Fact]
        public async Task UpdatePreview_SchemeMappedTest()
        {
            SchemesCollection.Items = Fixture.CreateMany<Scheme>(1);

            var scheme = SchemesCollection.Items.First();
            int differentiator = 0;
            Document document;
            string expectedAdditionalFooter, expectedBenefits, expectedCaseStudiesPreamble, expectedCost, expectedDescription, expectedDetailsPageOverride, expectedOffer, expectedResponsibility,
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

            var content = await ContentService.UpdatePreview();

            var actualScheme = content.Schemes.FirstOrDefault();
            Assert.NotNull(actualScheme);

            //todo: should really copy item before ContentService.Update() (or pick out fields), in case they get mutated
            // or even better add a test to check that they don't get mutated
            var expectedSourceScheme = SchemesCollection.Items.First();
            Assert.Equal(expectedSourceScheme.Url, actualScheme.Url);
            Assert.Equal(expectedSourceScheme.Name, actualScheme.Name);
            Assert.Equal(expectedSourceScheme.OfferHeader, actualScheme.OfferHeader);
            Assert.Equal(expectedSourceScheme.Size, actualScheme.Size);
            Assert.Equal(expectedAdditionalFooter, actualScheme.AdditionalFooter.Value);
            Assert.Equal(expectedBenefits, actualScheme.Benefits.Value);
            Assert.Equal(expectedCaseStudiesPreamble, actualScheme.CaseStudiesPreamble.Value);
            Assert.Equal(expectedCost, actualScheme.Cost.Value);
            Assert.Equal(expectedDescription, actualScheme.Description.Value);
            Assert.Equal(expectedDetailsPageOverride, actualScheme.DetailsPageOverride.Value);
            Assert.Equal(expectedOffer, actualScheme.Offer.Value);
            Assert.Equal(expectedResponsibility, actualScheme.Responsibility.Value);
            Assert.Equal(expectedShortBenefits, actualScheme.ShortBenefits.Value);
            Assert.Equal(expectedShortCost, actualScheme.ShortCost.Value);
            Assert.Equal(expectedShortDescription, actualScheme.ShortDescription.Value);
            Assert.Equal(expectedShortTime, actualScheme.ShortTime.Value);
            //todo: enumerable fields
        }

        // Contentful's .net library is not very test friendly: HtmlRenderer.ToHtml can't be mocked
        // we'd have to introduce a level of indirection to test this
        // or we _could_ make ToHtmlString in ContentService public and test it directly
        //[Theory]
        //[InlineData("\"", "“")]
        //public async Task Update_HtmlQuirksTest(string expectedHtmlStringValue, string unescapedHtml)
        //{
        //    var pages = Fixture.CreateMany<Page>(1).ToList();
        //    PagesCollection.Items = pages;

        //    A.CallTo(() => HtmlRenderer.ToHtml(A<Document>.Ignored))
        //        .Returns(unescapedHtml);

        //    var content = await ContentService.Update();

        //    Assert.Equal(expectedHtmlStringValue, content.Pages.First().Content.Value);
        //}

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
            var result = ContentService.ToNormalisedHtmlString(html);

            Assert.Equal(expectedHtmlStringValue, result.Value);
        }

        [Fact]
        public void Content_IsGeneratedContentBeforeUpdate()
        {
            var compareResult = CompareLogic.Compare(new GeneratedContent(), ContentService.Content);

            Assert.True(compareResult.AreEqual);
        }

        [Fact]
        public async Task Content_IsNotGeneratedContentAfterUpdate()
        {
            await ContentService.Update();

            var compareResult = CompareLogic.Compare(new GeneratedContent(), ContentService.Content);

            Assert.False(compareResult.AreEqual);
        }

        [Fact]
        public void PreviewContent_IsNullBeforeUpdate()
        {
            Assert.Null(ContentService.PreviewContent);
        }

        [Fact]
        public async Task CreateHtmlRenderer_RenderingNullContent()
        {
            var renderer = ContentService.CreateHtmlRenderer();

            var nullResult = await renderer.ToHtml(null);

            Assert.Equal(string.Empty, nullResult);
        }

        [Fact]
        public async Task CreateHtmlRenderer_RenderingEmptyContent()
        {
            var renderer = ContentService.CreateHtmlRenderer();
            var emptyDocument = new Document();

            var emptyResult = await renderer.ToHtml(emptyDocument);

            Assert.Equal(string.Empty, emptyResult);
        }
    }
}
