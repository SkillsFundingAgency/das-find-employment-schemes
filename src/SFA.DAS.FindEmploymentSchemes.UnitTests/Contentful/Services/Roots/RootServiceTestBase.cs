using AutoFixture;
using Contentful.Core.Models;
using Contentful.Core;
using System.Collections.Generic;
using FakeItEasy;
using Contentful.Core.Search;
using System.Threading;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{
    public class RootServiceTestBase
    {
        public Fixture Fixture { get; }
        public Document Document { get; set; }
        public string ExpectedContent { get; set; }
        public IContentfulClient ContentfulClient { get; set; }
        public HtmlRenderer HtmlRenderer { get; set; }

        public RootServiceTestBase()
        {
            Fixture = new Fixture();

            (Document, ExpectedContent) = SampleDocumentAndExpectedContent();

            Fixture.Inject(Document);

            ContentfulClient = A.Fake<IContentfulClient>();

            HtmlRenderer = A.Fake<HtmlRenderer>();
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
                        Content = new List<IContent> {new Text {Value = $"Gobble{differentiator}"}}
                    }
                }
            }, $"<h2>Gobble{differentiator}</h2>");
        }

        public void SetupContentfulClientCall<TApiModel>(
            IContentfulClient contentfulClient,
            ContentfulCollection<TApiModel> returnCollection)
        {
            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<TApiModel>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(returnCollection);
        }
    }
}