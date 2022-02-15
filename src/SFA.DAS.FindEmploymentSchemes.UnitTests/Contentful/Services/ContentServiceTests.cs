using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FakeItEasy;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services
{
    public class ContentServiceTests
    {
        public Fixture Fixture { get; }
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

            //Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            //    .ForEach(b => Fixture.Behaviors.Remove(b));
            //Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            //Fixture.Customizations.Add(
            //    new TypeRelay(
            //        typeof(IContent),
            //        typeof(Paragraph)));

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
            A.CallTo(() => ContentfulClient.GetEntries(A<QueryBuilder<Page>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(PagesCollection);

            CaseStudyPagesCollection = new ContentfulCollection<CaseStudyPage> { Items = Array.Empty<CaseStudyPage>() };
            A.CallTo(() => ContentfulClient.GetEntries(A<QueryBuilder<CaseStudyPage>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(CaseStudyPagesCollection);

            SchemesCollection = new ContentfulCollection<Scheme> { Items = Array.Empty<Scheme>() };
            A.CallTo(() => ContentfulClient.GetEntries(A<QueryBuilder<Scheme>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(SchemesCollection);

            FiltersCollection = new ContentfulCollection<Filter> { Items = Array.Empty<Filter>() };
            //A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Filter>>.Ignored, A<CancellationToken>.Ignored))
            //    .Returns(payFiltersCollection).Once()
            //    .Then
            //    .Returns(new ContentfulCollection<Filter>());

            // decouples us from the order of fetching filters
            A.CallTo(() => ContentfulClient.GetEntries(A<QueryBuilder<Filter>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(FiltersCollection);

            ContentService = new ContentService(ContentfulClientFactory, HtmlRenderer, Logger);

            CompareLogic = new CompareLogic();
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
