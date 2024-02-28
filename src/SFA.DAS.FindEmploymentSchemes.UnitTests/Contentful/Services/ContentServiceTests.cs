using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FakeItEasy;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public ISchemeService SchemeService { get; set; }
        public IPageService PageService { get; set; }
        public ICaseStudyPageService CaseStudyPageService { get; set; }
        public ISchemeFilterService SchemeFilterService { get; set; }
        public IInterimService InterimService { get; set; }
        public IContactService ContactService { get; set; }

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
            SchemeFilterService = A.Fake<ISchemeFilterService>();
            ContactService = A.Fake<IContactService>();
            InterimService = A.Fake<IInterimService>();

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

            var contentPages = Fixture.CreateMany<Page>(numberOfPages);
            A.CallTo(() => PageService.GetAll(ContentfulClient))
                .Returns(contentPages);

            var content = await ContentService.Update();

            Assert.NotNull(content.Pages);
            Assert.Equal(numberOfPages, content.Pages.Count());
        }

        [Fact]
        public async Task Update_PageTest()
        {
            const int numberOfPages = 1;

            Fixture.Inject(ExpectedContent);
            var contentPages = Fixture.CreateMany<Page>(numberOfPages).ToArray();
            A.CallTo(() => PageService.GetAll(ContentfulClient))
                .Returns(contentPages);

            var content = await ContentService.Update();

            var actualPage = content.Pages.FirstOrDefault();
            Assert.NotNull(actualPage);

            var expectedSourcePage = contentPages.First();
            Assert.Equal(expectedSourcePage.Title, actualPage.Title);
            Assert.Equal(expectedSourcePage.Url, actualPage.Url);
            Assert.Equal(ExpectedContent.Value, actualPage.Content.Value);
        }

        [Fact]
        public async Task Update_SameNumberOfCaseStudyPagesTest()
        {
            const int numberOfCaseStudyPages = 3;

            var contentCaseStudyPages = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages).ToArray();
            A.CallTo(() => CaseStudyPageService.GetAll(ContentfulClient, A<IEnumerable<Scheme>>._))
                .Returns(contentCaseStudyPages);

            var content = await ContentService.Update();

            Assert.NotNull(content.CaseStudyPages);
            Assert.Equal(numberOfCaseStudyPages, content.CaseStudyPages.Count());
        }

        [Fact]
        public async Task Update_CaseStudyPageTest()
        {
            const int numberOfCaseStudyPages = 1;

            Fixture.Inject(ExpectedContent);
            var contentCaseStudyPages = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages).ToArray();
            A.CallTo(() => CaseStudyPageService.GetAll(ContentfulClient, A<IEnumerable<Scheme>>._))
                .Returns(contentCaseStudyPages);

            var content = await ContentService.Update();

            var actualCaseStudyPage = content.CaseStudyPages.FirstOrDefault();
            Assert.NotNull(actualCaseStudyPage);

            var expectedSourceCaseStudyPage = contentCaseStudyPages.First();
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

            var contentSchemes = Fixture.CreateMany<Scheme>(numberOfSchemes).ToArray();
            A.CallTo(() => SchemeService.GetAll(ContentfulClient))
                .Returns(contentSchemes);

            var content = await ContentService.Update();

            Assert.NotNull(content.Schemes);
            Assert.Equal(numberOfSchemes, content.Schemes.Count());
        }

        [Fact]
        public async Task Update_SchemeTest()
        {
            Fixture.Inject(ExpectedContent);
            var contentSchemes = Fixture.CreateMany<Scheme>(1).ToArray();
            A.CallTo(() => SchemeService.GetAll(ContentfulClient))
                .Returns(contentSchemes);

            var content = await ContentService.Update();

            var actualScheme = content.Schemes.FirstOrDefault();
            Assert.NotNull(actualScheme);

            var expectedSourceScheme = contentSchemes.First();
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

            var contentPages = Fixture.CreateMany<Page>(numberOfPages);
            A.CallTo(() => PageService.GetAll(PreviewContentfulClient))
                .Returns(contentPages);

            var content = await ContentService.UpdatePreview();

            Assert.NotNull(content.Pages);
            Assert.Equal(numberOfPages, content.Pages.Count());
        }

        [Fact]
        public async Task UpdatePreview_PageTest()
        {
            const int numberOfPages = 1;

            Fixture.Inject(ExpectedContent);
            var contentPages = Fixture.CreateMany<Page>(numberOfPages).ToArray();
            A.CallTo(() => PageService.GetAll(PreviewContentfulClient))
                .Returns(contentPages);

            var content = await ContentService.UpdatePreview();

            var actualPage = content.Pages.FirstOrDefault();
            Assert.NotNull(actualPage);

            var expectedSourcePage = contentPages.First();
            Assert.Equal(expectedSourcePage.Title, actualPage.Title);
            Assert.Equal(expectedSourcePage.Url, actualPage.Url);
            Assert.Equal(ExpectedContent.Value, actualPage.Content.Value);
        }

        [Fact]
        public async Task UpdatePreview_SameNumberOfCaseStudyPagesTest()
        {
            const int numberOfCaseStudyPages = 3;

            var contentCaseStudyPages = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages).ToArray();
            A.CallTo(() => CaseStudyPageService.GetAll(PreviewContentfulClient, A<IEnumerable<Scheme>>._))
                .Returns(contentCaseStudyPages);

            var content = await ContentService.UpdatePreview();

            Assert.NotNull(content.CaseStudyPages);
            Assert.Equal(numberOfCaseStudyPages, content.CaseStudyPages.Count());
        }

        [Fact]
        public async Task UpdatePreview_CaseStudyPageMappedTest()
        {
            const int numberOfCaseStudyPages = 1;

            Fixture.Inject(ExpectedContent);
            var contentCaseStudyPages = Fixture.CreateMany<CaseStudyPage>(numberOfCaseStudyPages).ToArray();
            A.CallTo(() => CaseStudyPageService.GetAll(PreviewContentfulClient, A<IEnumerable<Scheme>>._))
                .Returns(contentCaseStudyPages);

            var content = await ContentService.UpdatePreview();

            var actualCaseStudyPage = content.CaseStudyPages.FirstOrDefault();
            Assert.NotNull(actualCaseStudyPage);

            var expectedSourceCaseStudyPage = contentCaseStudyPages.First();
            Assert.Equal(expectedSourceCaseStudyPage.Title, actualCaseStudyPage.Title);
            Assert.Equal(expectedSourceCaseStudyPage.Url, actualCaseStudyPage.Url);
            //Assert.Equal(SchemesCollection.Items.First().Url, actualCaseStudyPage.Scheme.Url);
            Assert.Equal(ExpectedContent.Value, actualCaseStudyPage.Content.Value);
        }

        [Fact]
        public async Task UpdatePreview_SameNumberOfSchemesTest()
        {
            const int numberOfSchemes = 3;

            var contentSchemes = Fixture.CreateMany<Scheme>(numberOfSchemes).ToArray();
            A.CallTo(() => SchemeService.GetAll(PreviewContentfulClient))
                .Returns(contentSchemes);

            var content = await ContentService.UpdatePreview();

            Assert.NotNull(content.Schemes);
            Assert.Equal(numberOfSchemes, content.Schemes.Count());
        }

        [Fact]
        public async Task UpdatePreview_SchemeMappedTest()
        {
            Fixture.Inject(ExpectedContent);
            var contentSchemes = Fixture.CreateMany<Scheme>(1).ToArray();
            A.CallTo(() => SchemeService.GetAll(PreviewContentfulClient))
                .Returns(contentSchemes);

            var content = await ContentService.UpdatePreview();

            var actualScheme = content.Schemes.FirstOrDefault();
            Assert.NotNull(actualScheme);

            var expectedSourceScheme = contentSchemes.First();
            var result = CompareLogic.Compare(expectedSourceScheme, actualScheme);
            Assert.True(result.AreEqual, result.DifferencesString);
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

        private void CreateContentService()
        {
            ContentService = new ContentService(
                ContentfulClientFactory,
                SchemeService,
                PageService,
                CaseStudyPageService,
                SchemeFilterService,
                ContactService,
                InterimService,
                Logger);
        }

    }

}
