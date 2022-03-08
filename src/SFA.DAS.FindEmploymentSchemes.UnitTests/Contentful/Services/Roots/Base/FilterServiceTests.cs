using System.Linq;
using AutoFixture;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using System.Threading.Tasks;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots.Base
{
    public class FilterServiceTests : RootServiceTestBase<Filter, FilterService>
    {
        public FilterService FilterService { get; set; }

        public FilterServiceTests()
        {
            FilterService = new FilterService(HtmlRenderer, "name", "description", "contentfulTypeName", "prefix");
        }

        [Theory]
        [InlineData("prefix--the-name", "the name")]
        [InlineData("prefix--thename", "thename")]
        [InlineData("prefix--the-name", "the-name")]
        //todo: stop double spaces, so code doesn't get confused with prefix/name separator?
        [InlineData("prefix--the--name", "the  name")]
        [InlineData("prefix--", "")]
        [InlineData("prefix--1234567890-qwertyuiop-asdfghjkl-zxcvbnm", "1234567890 qwertyuiop asdfghjkl zxcvbnm")]
        public async Task Update_FilterIdTests(string expectedFilterAspectId, string filterName)
        {
            var filters = Fixture.CreateMany<Filter>(1).ToArray();
            filters.First().Name = filterName;
            ContentfulCollection.Items = filters;

            var filter = await FilterService.Get(ContentfulClient);

            Assert.Equal(expectedFilterAspectId, filter.Aspects.First().Id);
        }
    }
}
