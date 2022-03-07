using AutoFixture;
using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Contentful.Core;
using Contentful.Core.Search;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{
    public class PageServiceTests
    {
        public Fixture Fixture { get; }
        public Document Document { get; set; }
        public string ExpectedContent { get; set; }
        public IContentfulClient ContentfulClient { get; set; }
        public ContentfulCollection<Page> PagesCollection { get; set; }
        public HtmlRenderer HtmlRenderer { get; set; }
        public ILogger<PageService> Logger { get; set; }
        public PageService PageService { get; set; }

        public PageServiceTests()
        {
            Fixture = new Fixture();

            (Document, ExpectedContent) = SampleDocumentAndExpectedContent();

            Fixture.Inject(Document);

            ContentfulClient = A.Fake<IContentfulClient>();

            PagesCollection = new ContentfulCollection<Page> { Items = Array.Empty<Page>() };
            SetupContentfulClientCalls(ContentfulClient);

            HtmlRenderer = A.Fake<HtmlRenderer>();
            Logger = A.Fake<ILogger<PageService>>();

            PageService = new PageService(HtmlRenderer, Logger);
        }

        [Fact]
        public async Task GetAll_PageMappedTest()
        {
            const int numberOfPages = 1;

            PagesCollection.Items = Fixture.CreateMany<Page>(numberOfPages);

            var pages = await PageService.GetAll(ContentfulClient);

            var actualPage = pages.FirstOrDefault();
            Assert.NotNull(actualPage);

            var expectedSourcePage = PagesCollection.Items.First();
            Assert.Equal(expectedSourcePage.Title, actualPage.Title);
            Assert.Equal(expectedSourcePage.Url, actualPage.Url);
            Assert.Equal(ExpectedContent, actualPage.Content.Value);
        }

        private void SetupContentfulClientCalls(IContentfulClient contentfulClient)
        {
            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Page>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(PagesCollection);
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
    }
}