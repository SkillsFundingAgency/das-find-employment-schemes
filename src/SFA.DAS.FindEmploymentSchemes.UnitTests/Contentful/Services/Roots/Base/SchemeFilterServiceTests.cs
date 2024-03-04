using System.Linq;
using AutoFixture;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using System.Threading.Tasks;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Contentful.Services.Roots.Base
{

    public class SchemeFilterServiceTests : RootServiceTestBase<SchemeFilter, SchemeFilterService>
    {

        public SchemeFilterService SchemeFilterService { get; set; }

        public SchemeFilterServiceTests()
        {

            SchemeFilterService = new SchemeFilterService(
                
                A.Fake<ILogger<SchemeFilterService>>()
                
            );
       
        }

        [Fact]
        public async Task Get_PropertiesTest()
        {

            ContentfulCollection.Items = Fixture.CreateMany<SchemeFilter>(1);

            var filters = await SchemeFilterService.GetSchemeFilters(ContentfulClient);

            //Assert.Equal("description", filter.Description);

            Assert.Single(filters);

        }

        [Fact]
        public async Task Get_FiltersOrderTests()
        {
            var filters = Fixture.CreateMany<SchemeFilter>(3).ToArray();
            filters[0].SchemeFilterOrder = 200;
            filters[1].SchemeFilterOrder = 100;
            filters[2].SchemeFilterOrder = 300;
            ContentfulCollection.Items = filters;

            var filter = await SchemeFilterService.GetSchemeFilters(ContentfulClient);

            Assert.Equal(filters[1].SchemeFilterDescription, filter[0].SchemeFilterDescription);
            Assert.Equal(filters[0].SchemeFilterDescription, filter[1].SchemeFilterDescription);
            Assert.Equal(filters[2].SchemeFilterDescription, filter[2].SchemeFilterDescription);
        }
    }
}
