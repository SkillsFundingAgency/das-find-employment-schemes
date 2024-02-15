using FakeItEasy;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class FilterServiceTests
    {

        private readonly BetaBanner BetaBanner;

        public FilterServiceTests()
        {

            BetaBanner = new BetaBanner() { BetaBannerID = "BetaBannerID", BetaBannerTitle = "BetaBannerTitle", BetaBannerContent = null };

        }

        [Theory]
        [ClassData(typeof(FilterServiceTestData))]
        public void ApplyFilters_Result(IEnumerable<Scheme> expectedSchemes, SchemeFilterModel filters)
        {
            IContentService contentService = A.Fake<IContentService>();
            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();
            ILogger<FilterService> filterServiceLogger = A.Fake<ILogger<FilterService>>();

            FilterService service = new FilterService(contentService, schemesModelService, filterServiceLogger);
            HomeModel model = A.Fake<HomeModel>(x => x.WithArgumentsForConstructor(() => new HomeModel(null, expectedSchemes, null, new List<InterimMenuItem>(), BetaBanner, false, "")));
            A.CallTo(() => contentService.Content).Returns(new GeneratedContent());

            HomeModel result = service.ApplyFilter(filters);
            Assert.True(expectedSchemes.Count() == result.Schemes.Count());
        }

        [Fact(DisplayName = "FilterService - ApplyFilters returns correct filter URL")]
        public void FilterService_ApplyFilters_Returns_Correct_Filter_URL()
        {

            IContentService contentService = A.Fake<IContentService>();

            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();

            ILogger<FilterService> filterServiceLogger = A.Fake<ILogger<FilterService>>();

            FilterService service = new FilterService(contentService, schemesModelService, filterServiceLogger);

            A.CallTo(() => contentService.Content).Returns(new GeneratedContent());

            HomeModel result = service.ApplyFilter(
                
                new SchemeFilterModel()
            
                {

                    FilterAspects = new string[1] { "aspect" }

                }
            
            );

            Assert.Equal("filters=aspect", result.SelectedFilters);

        }

        public class FilterServiceTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                string fourToTwelveMonths = "scheme-length--4-months-to-12-months";
                string yearOrMore = "scheme-length--a-year-or-more";
                string unpaid = "pay--unpaid";

                var generatedContent = new GeneratedContent();

                yield return new object[] {
                    generatedContent.Schemes,
                    new SchemeFilterModel()
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(fourToTwelveMonths)),
                    new SchemeFilterModel { FilterAspects = new[] { fourToTwelveMonths }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(yearOrMore)),
                    new SchemeFilterModel { FilterAspects = new[] { yearOrMore }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(unpaid)),
                    new SchemeFilterModel { FilterAspects = new[] { unpaid }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(yearOrMore) && s.FilterAspects.Contains(unpaid)),
                    new SchemeFilterModel { FilterAspects = new[] { yearOrMore, unpaid }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(fourToTwelveMonths) && s.FilterAspects.Contains(yearOrMore)),
                    new SchemeFilterModel { FilterAspects = new[] { fourToTwelveMonths, yearOrMore }}
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}