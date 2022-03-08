using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core;
using Contentful.Core.Models;
using FakeItEasy;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services
{
    public class ContentServiceTests
    {
        public Fixture Fixture { get; }
        public Document Document { get; set; }
        public HtmlString ExpectedContent { get; set; }
        public IContentfulClientFactory ContentfulClientFactory { get; set; }
        public IContentfulClient ContentfulClient { get; set; }
        public IContentfulClient PreviewContentfulClient { get; set; }
        public HtmlRenderer HtmlRenderer { get; set; }
        public ILogger<ContentService> Logger { get; set; }
        //todo:local?
        public IEnumerable<Page> ContentPages { get; set; }
        public IEnumerable<CaseStudyPage> ContentCaseStudyPages { get; set; }
        public IEnumerable<Scheme> ContentSchemes { get; set; }
        public ISchemeService SchemeService { get; set; }
        public IPageService PageService { get; set; }
        public ICaseStudyPageService CaseStudyPageService { get; set; }
        public IMotivationFilterService MotivationFilterService { get; set; }
        public IPayFilterService PayFilterService { get; set; }
        public ISchemeLengthFilterService SchemeLengthFilterService { get; set; }
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

            SchemeService = A.Fake<ISchemeService>();
            PageService = A.Fake<IPageService>();
            CaseStudyPageService = A.Fake<ICaseStudyPageService>();
            MotivationFilterService = A.Fake<IMotivationFilterService>();
            PayFilterService = A.Fake<IPayFilterService>();
            SchemeLengthFilterService = A.Fake<ISchemeLengthFilterService>();

            CreateContentService();

            CompareLogic = new CompareLogic();
        }

        private (Document, HtmlString) SampleDocumentAndExpectedContent(int differentiator = 0)
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
            }, new HtmlString($"<h2>Gobble{differentiator}</h2>"));
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

        [Fact]
        public async Task Update_MissingContentfulClientThrowsExceptionTest()
        {
            A.CallTo(() => ContentfulClientFactory.ContentfulClient)
                .Returns(null);

            CreateContentService();

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

            ContentPages = Fixture.CreateMany<Page>(numberOfPages);
            A.CallTo(() => PageService.GetAll(ContentfulClient))
                .Returns(ContentPages);

            var content = await ContentService.Update();

            Assert.NotNull(content.Pages);
            Assert.Equal(numberOfPages, content.Pages.Count());
        }

        [Fact]
        public async Task Update_PageTest()
        {
            const int numberOfPages = 1;

            Fixture.Inject(ExpectedContent);
            ContentPages = Fixture.CreateMany<Page>(numberOfPages).ToArray();
            A.CallTo(() => PageService.GetAll(ContentfulClient))
                .Returns(ContentPages);

            var content = await ContentService.Update();

            var actualPage = content.Pages.FirstOrDefault();
            Assert.NotNull(actualPage);

            var expectedSourcePage = ContentPages.First();
            Assert.Equal(expectedSourcePage.Title, actualPage.Title);
            Assert.Equal(expectedSourcePage.Url, actualPage.Url);
            Assert.Equal(ExpectedContent.Value, actualPage.Content.Value);
        }

        [Fact]
        public async Task Update_SameNumberOfCaseStudyPagesTest()
        {
            const int numberOfCaseStudyPages = 3;

            ContentCaseStudyPages = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages).ToArray();
            A.CallTo(() => CaseStudyPageService.GetAll(ContentfulClient, A<IEnumerable<Scheme>>._))
                .Returns(ContentCaseStudyPages);

            var content = await ContentService.Update();

            Assert.NotNull(content.CaseStudyPages);
            Assert.Equal(numberOfCaseStudyPages, content.CaseStudyPages.Count());
        }

        [Fact]
        public async Task Update_CaseStudyPageTest()
        {
            const int numberOfCaseStudyPages = 1;

            Fixture.Inject(ExpectedContent);
            ContentCaseStudyPages = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages).ToArray();
            A.CallTo(() => CaseStudyPageService.GetAll(ContentfulClient, A<IEnumerable<Scheme>>._))
                .Returns(ContentCaseStudyPages);

            var content = await ContentService.Update();

            var actualCaseStudyPage = content.CaseStudyPages.FirstOrDefault();
            Assert.NotNull(actualCaseStudyPage);

            var expectedSourceCaseStudyPage = ContentCaseStudyPages.First();
            Assert.Equal(expectedSourceCaseStudyPage.Title, actualCaseStudyPage.Title);
            Assert.Equal(expectedSourceCaseStudyPage.Url, actualCaseStudyPage.Url);
            //todo:
            //Assert.Equal(SchemesCollection.Items.First().Url, actualCaseStudyPage.Scheme.Url);
            Assert.Equal(ExpectedContent.Value, actualCaseStudyPage.Content.Value);
        }

        [Fact]
        public async Task Update_SameNumberOfSchemesTest()
        {
            const int numberOfSchemes = 3;

            ContentSchemes = Fixture.CreateMany<Scheme>(numberOfSchemes).ToArray();
            A.CallTo(() => SchemeService.GetAll(ContentfulClient))
                .Returns(ContentSchemes);

            var content = await ContentService.Update();

            Assert.NotNull(content.Schemes);
            Assert.Equal(numberOfSchemes, content.Schemes.Count());
        }

        [Fact]
        public async Task Update_SchemeTest()
        {
            Fixture.Inject(ExpectedContent);
            ContentSchemes = Fixture.CreateMany<Scheme>(1).ToArray();
            A.CallTo(() => SchemeService.GetAll(ContentfulClient))
                .Returns(ContentSchemes);

            var content = await ContentService.Update();

            var actualScheme = content.Schemes.FirstOrDefault();
            Assert.NotNull(actualScheme);

            var expectedSourceScheme = ContentSchemes.First();
            var result = CompareLogic.Compare(expectedSourceScheme, actualScheme);
            Assert.True(result.AreEqual, result.DifferencesString);
        }

        [Fact]
        public async Task UpdatePreview_MissingContentfulClientThrowsExceptionTest()
        {
            A.CallTo(() => ContentfulClientFactory.PreviewContentfulClient)
                .Returns(null);

            CreateContentService();

            await Assert.ThrowsAsync<ContentServiceException>(() => ContentService.UpdatePreview());
        }

        [Fact]
        public async Task UpdatePreview_PreviewContentUpdatedEventIsRaisedTest()
        {
            bool eventWasRaised = false;
            ContentService.PreviewContentUpdated += (sender, args) => { eventWasRaised = true; };

            await ContentService.UpdatePreview();

            Assert.True(eventWasRaised);
        }

        [Fact]
        public async Task UpdatePreview_SameNumberOfPagesTest()
        {
            const int numberOfPages = 3;

            ContentPages = Fixture.CreateMany<Page>(numberOfPages);
            A.CallTo(() => PageService.GetAll(PreviewContentfulClient))
                .Returns(ContentPages);

            var content = await ContentService.UpdatePreview();

            Assert.NotNull(content.Pages);
            Assert.Equal(numberOfPages, content.Pages.Count());
        }

        [Fact]
        public async Task UpdatePreview_PageTest()
        {
            const int numberOfPages = 1;

            Fixture.Inject(ExpectedContent);
            ContentPages = Fixture.CreateMany<Page>(numberOfPages).ToArray();
            A.CallTo(() => PageService.GetAll(PreviewContentfulClient))
                .Returns(ContentPages);

            var content = await ContentService.UpdatePreview();

            var actualPage = content.Pages.FirstOrDefault();
            Assert.NotNull(actualPage);

            var expectedSourcePage = ContentPages.First();
            Assert.Equal(expectedSourcePage.Title, actualPage.Title);
            Assert.Equal(expectedSourcePage.Url, actualPage.Url);
            Assert.Equal(ExpectedContent.Value, actualPage.Content.Value);
        }

        [Fact]
        public async Task UpdatePreview_SameNumberOfCaseStudyPagesTest()
        {
            const int numberOfCaseStudyPages = 3;

            ContentCaseStudyPages = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages).ToArray();
            A.CallTo(() => CaseStudyPageService.GetAll(PreviewContentfulClient, A<IEnumerable<Scheme>>._))
                .Returns(ContentCaseStudyPages);

            var content = await ContentService.UpdatePreview();

            Assert.NotNull(content.CaseStudyPages);
            Assert.Equal(numberOfCaseStudyPages, content.CaseStudyPages.Count());
        }

        [Fact]
        public async Task UpdatePreview_CaseStudyPageMappedTest()
        {
            const int numberOfCaseStudyPages = 1;

            Fixture.Inject(ExpectedContent);
            ContentCaseStudyPages = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages).ToArray();
            A.CallTo(() => CaseStudyPageService.GetAll(PreviewContentfulClient, A<IEnumerable<Scheme>>._))
                .Returns(ContentCaseStudyPages);

            var content = await ContentService.UpdatePreview();

            var actualCaseStudyPage = content.CaseStudyPages.FirstOrDefault();
            Assert.NotNull(actualCaseStudyPage);

            var expectedSourceCaseStudyPage = ContentCaseStudyPages.First();
            Assert.Equal(expectedSourceCaseStudyPage.Title, actualCaseStudyPage.Title);
            Assert.Equal(expectedSourceCaseStudyPage.Url, actualCaseStudyPage.Url);
            //Assert.Equal(SchemesCollection.Items.First().Url, actualCaseStudyPage.Scheme.Url);
            Assert.Equal(ExpectedContent.Value, actualCaseStudyPage.Content.Value);
        }

        [Fact]
        public async Task UpdatePreview_SameNumberOfSchemesTest()
        {
            const int numberOfSchemes = 3;

            ContentSchemes = Fixture.CreateMany<Scheme>(numberOfSchemes).ToArray();
            A.CallTo(() => SchemeService.GetAll(PreviewContentfulClient))
                .Returns(ContentSchemes);

            var content = await ContentService.UpdatePreview();

            Assert.NotNull(content.Schemes);
            Assert.Equal(numberOfSchemes, content.Schemes.Count());
        }

        [Fact]
        public async Task UpdatePreview_SchemeMappedTest()
        {
            Fixture.Inject(ExpectedContent);
            ContentSchemes = Fixture.CreateMany<Scheme>(1).ToArray();
            A.CallTo(() => SchemeService.GetAll(PreviewContentfulClient))
                .Returns(ContentSchemes);

            var content = await ContentService.UpdatePreview();

            var actualScheme = content.Schemes.FirstOrDefault();
            Assert.NotNull(actualScheme);

            var expectedSourceScheme = ContentSchemes.First();
            var result = CompareLogic.Compare(expectedSourceScheme, actualScheme);
            Assert.True(result.AreEqual, result.DifferencesString);
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

        private void CreateContentService()
        {
            ContentService = new ContentService(
                ContentfulClientFactory,
                SchemeService,
                PageService,
                CaseStudyPageService,
                MotivationFilterService,
                PayFilterService,
                SchemeLengthFilterService,
                Logger);
        }
    }
}
