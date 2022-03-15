using AutoFixture;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using Contentful.Core.Models;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots
{
    public class PageServiceTests : RootServiceTestBase<Page, PageService>
    {
        public PageService PageService { get; set; }

        public PageServiceTests()
        {
            PageService = new PageService(HtmlRenderer, Logger);
        }

        [Fact]
        public async Task GetAll_SameNumberOfPagesTest()
        {
            const int numberOfPages = 3;

            ContentfulCollection.Items = Fixture.CreateMany<Page>(numberOfPages);

            var pages = await PageService.GetAll(ContentfulClient);

            Assert.NotNull(pages);
            Assert.Equal(numberOfPages, pages.Count());
        }

        [Fact]
        public async Task GetAll_PageTest()
        {
            const int numberOfPages = 1;

            ContentfulCollection.Items = Fixture.CreateMany<Page>(numberOfPages);

            var pages = await PageService.GetAll(ContentfulClient);

            var actualPage = pages.FirstOrDefault();
            Assert.NotNull(actualPage);

            var expectedSourcePage = ContentfulCollection.Items.First();
            Assert.Equal(expectedSourcePage.Title, actualPage.Title);
            Assert.Equal(expectedSourcePage.Url, actualPage.Url);
            Assert.Equal(ExpectedContent.Value, actualPage.Content.Value);
        }

        [Fact]
        public async Task GetAll_NullUrlsFilteredOutTest()
        {
            const int numberOfPages = 3;

            var pages = Fixture.CreateMany<Page>(numberOfPages).ToArray();
            pages[1].Url = null;
            ContentfulCollection.Items = pages;

            var pagesResult = await PageService.GetAll(ContentfulClient);

            Assert.NotNull(pages);
            Assert.Equal(numberOfPages-1, pagesResult.Count());
        }

        [Fact]
        public async Task GetAll_EmptyUrlsFilteredOutTest()
        {
            const int numberOfPages = 3;

            var pages = Fixture.CreateMany<Page>(numberOfPages).ToArray();
            pages[0].Url = "";
            pages[2].Url = "";
            ContentfulCollection.Items = pages;

            var pagesResult = await PageService.GetAll(ContentfulClient);

            Assert.NotNull(pages);
            Assert.Single(pagesResult);
        }
    }
}