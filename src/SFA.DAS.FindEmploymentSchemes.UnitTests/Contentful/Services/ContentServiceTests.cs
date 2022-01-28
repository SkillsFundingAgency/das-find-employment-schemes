using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services
{
    public class ContentServiceTests
    {
        public Fixture Fixture { get; }
        public IContentfulClient ContentfulClient { get; set; }
        public HtmlRenderer HtmlRenderer { get; set; }
        public ILogger<ContentService> Logger { get; set; }
        public ContentfulCollection<Page> PagesCollection { get; set; }
        public ContentfulCollection<Scheme> SchemesCollection { get; set; }
        public ContentfulCollection<Filter> FiltersCollection { get; set; }

        public ContentServiceTests()
        {
            Fixture = new Fixture();

            ContentfulClient = A.Fake<IContentfulClient>();
            HtmlRenderer = A.Fake<HtmlRenderer>();
            Logger = A.Fake<ILogger<ContentService>>();

            PagesCollection = new ContentfulCollection<Page> { Items = Array.Empty<Page>() };
            A.CallTo(() => ContentfulClient.GetEntries(A<QueryBuilder<Page>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(PagesCollection);

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
        }

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

            var contentService = new ContentService(ContentfulClient, HtmlRenderer, Logger);

            var content = await contentService.Update();

            Assert.Equal(expectedFilterAspectId, content.PayFilter.Aspects.First().Id);
        }

        //public async Task Content_IsGeneratedContentBeforeUpdate(string expectedFilterAspectId, string filterName)
        //{

        //}
    }
}
