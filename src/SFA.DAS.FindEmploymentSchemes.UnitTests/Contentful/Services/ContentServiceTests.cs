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
        [Theory]
        [InlineData("pay--the-name", "the name")]
        [InlineData("pay--thename", "thename")]
        [InlineData("pay--the-name", "the-name")]
        //todo: stop double spaces, so code doesn't get confused with prefix/name separator?
        [InlineData("pay--the--name", "the  name")]
        [InlineData("pay--", "")]
        [InlineData("pay--1234567890-qwertyuiop-asdfghjkl-zxcvbnm", "1234567890 qwertyuiop asdfghjkl zxcvbnm")]
        public async Task ToContentScheme_FilterIdTests(string expectedFilterAspectId, string filterName)
        {
            var contentfulClient = A.Fake<IContentfulClient>();
            var htmlRenderer = A.Fake<HtmlRenderer>();
            var logger = A.Fake<ILogger<ContentService>>();

            var fixture = new Fixture();

            var pagesCollection = new ContentfulCollection<Page> {Items = Array.Empty<Page>()};
            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Page>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(pagesCollection);

            var schemesCollection = new ContentfulCollection<Scheme> { Items = Array.Empty<Scheme>() };
            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Scheme>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(schemesCollection);

            var payFilters = fixture.CreateMany<Filter>(1).ToList();
            payFilters.First().Name = filterName;

            var payFiltersCollection = new ContentfulCollection<Filter>
            {
                Items = payFilters
            };

            //A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Filter>>.Ignored, A<CancellationToken>.Ignored))
            //    .Returns(payFiltersCollection).Once()
            //    .Then
            //    .Returns(new ContentfulCollection<Filter>());

            // decouples us from the order of fetching filters
            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<Filter>>.Ignored, A<CancellationToken>.Ignored))
                .Returns(payFiltersCollection);

            var contentService = new ContentService(contentfulClient, htmlRenderer, logger);

            var content = await contentService.Update();

            Assert.Equal(expectedFilterAspectId, content.PayFilter.Aspects.First().Id);
        }
    }
}
