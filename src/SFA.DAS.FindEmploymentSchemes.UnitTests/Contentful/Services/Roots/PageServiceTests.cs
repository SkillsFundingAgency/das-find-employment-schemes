using AutoFixture;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{
    public class PageServiceTests : RootServiceTestBase<Page>
    {
        public ILogger<PageService> Logger { get; set; }
        public PageService PageService { get; set; }

        public PageServiceTests()
        {
            Logger = A.Fake<ILogger<PageService>>();

            PageService = new PageService(HtmlRenderer, Logger);
        }

        [Fact]
        public async Task GetAll_PageMappedTest()
        {
            const int numberOfPages = 1;

            ContentfulCollection.Items = Fixture.CreateMany<Page>(numberOfPages);

            var pages = await PageService.GetAll(ContentfulClient);

            var actualPage = pages.FirstOrDefault();
            Assert.NotNull(actualPage);

            var expectedSourcePage = ContentfulCollection.Items.First();
            Assert.Equal(expectedSourcePage.Title, actualPage.Title);
            Assert.Equal(expectedSourcePage.Url, actualPage.Url);
            Assert.Equal(ExpectedContent, actualPage.Content.Value);
        }
    }
}