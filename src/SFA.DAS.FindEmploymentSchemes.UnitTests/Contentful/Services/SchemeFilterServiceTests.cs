using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services
{
    public class SchemeFilterServiceTests
    {

        public HtmlRenderer htmlRenderer { get; set; }

        public ILogger<SchemeFilterService> Logger { get; set; }

        public SchemeFilterServiceTests()
        {

            htmlRenderer = A.Fake<HtmlRenderer>();

            Logger = A.Fake<ILogger<SchemeFilterService>>();

        }

        [Fact(DisplayName = "SchemeFilterService - GetSchemeFilters - WithMatchingPage - ReturnsContactPage")]
        public async Task SchemeFilterService_GetSchemeFilters_WithMatchingSchemeFilters_ReturnsSchemeFilters()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var contactPageService = new SchemeFilterService(Logger);

            var entries = new ContentfulCollection<SchemeFilter>();

            entries.Items = [
                
                new SchemeFilter() 
            
                { 
                
                    SchemeFilterName = "SchemeFilterName",
                    
                    SchemeFilterPrefix = "SchemeFilterPrefix",

                    SchemeFilterDescription = "SchemeFilterDescription",

                    SchemeFilterOrder = 1
            
                }
            
            ];

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<SchemeFilter>>._, A<CancellationToken>._)).Returns(entries);

            var result = await contactPageService.GetSchemeFilters(contentfulClient);

            Assert.Single(result);

            Assert.Equal("SchemeFilterName", result.First().SchemeFilterName);

            Assert.Equal("SchemeFilterPrefix", result.First().SchemeFilterPrefix);

            Assert.Equal("SchemeFilterDescription", result.First().SchemeFilterDescription);

            Assert.Equal(1, result.First().SchemeFilterOrder);

        }

        [Fact(DisplayName = "SchemeFilterService - GetSchemeFilters - WithNoMatchingSchemeFilters - ReturnsNull")]
        public async Task SchemeFilterService_GetSchemeFilters_WithNoMatchingSchemeFilters_ReturnsNull()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var contactPageService = new SchemeFilterService(Logger);

            var fakeResult = new ContentfulCollection<SchemeFilter> { Items = new List<SchemeFilter>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<SchemeFilter>>._, A<CancellationToken>._)).Returns(fakeResult);

            var result = await contactPageService.GetSchemeFilters(contentfulClient);

            Assert.Empty(result);

        }

        [Fact(DisplayName = "SchemeFilterService - GetSchemeFilters - WithException - ReturnsNull")]
        public async Task SchemeFilterService_GetSchemeFilters_WithException_ReturnsNull()
        {

            var contentfulClient = A.Fake<IContentfulClient>();

            var contactPageService = new SchemeFilterService(Logger);

            var fakeResult = new ContentfulCollection<SchemeFilter> { Items = new List<SchemeFilter>() };

            A.CallTo(() => contentfulClient.GetEntries(A<QueryBuilder<SchemeFilter>>._, A<CancellationToken>._)).Throws(new Exception());

            var result = await contactPageService.GetSchemeFilters(contentfulClient);

            Assert.Empty(result);

        }

    }
}
