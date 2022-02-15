using Contentful.Core;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using System.Net.Http;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services
{
    public class ContentfulClientFactoryTests
    {
        public IContentfulClient ContentfulClient { get; set; }
        public IContentfulClient PreviewContentfulClient { get; set; }

        public ContentfulClientFactoryTests()
        {
            var httpClient = new HttpClient();
            ContentfulClient = new ContentfulClient(httpClient, "", "", "", false);
            PreviewContentfulClient = new ContentfulClient(httpClient, "", "", "", true);
        }

        [Fact]
        public void ContentfulClient_IsNonPreviewClientTest()
        {
            var bothTypesOfClient = new[] {ContentfulClient, PreviewContentfulClient};
            var contentfulClientFactory = new ContentfulClientFactory(bothTypesOfClient);

            Assert.Equal(ContentfulClient, contentfulClientFactory.ContentfulClient);
        }

        [Fact]
        public void PreviewContentfulClient_IsPreviewClientTest()
        {
            var bothTypesOfClient = new[] { ContentfulClient, PreviewContentfulClient };
            var contentfulClientFactory = new ContentfulClientFactory(bothTypesOfClient);

            Assert.Equal(PreviewContentfulClient, contentfulClientFactory.PreviewContentfulClient);
        }
    }
}
