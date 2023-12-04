using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class FilterServiceTests
    {
        [Theory]
        [ClassData(typeof(FilterServiceTestData))]
        public void ApplyFilters_Result(IEnumerable<Scheme> expectedSchemes, SchemeFilterModel filters)
        {
            IContentService contentService = A.Fake<IContentService>();
            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();
            ILogger<FilterService> filterServiceLogger = A.Fake<ILogger<FilterService>>();

            FilterService service = new FilterService(contentService, schemesModelService, filterServiceLogger);
            HomeModel model = A.Fake<HomeModel>(x => x.WithArgumentsForConstructor(() => new HomeModel(null, expectedSchemes, null, false, "")));
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

                    Pay = new string[1] { "Pay" },

                    SchemeLength = new string[1] { "Duration" },

                    Motivations = new string[1] { "Motivation" }

                }
            
            );

            Assert.Equal("pay=Pay&duration=Duration&motivation=Motivation", result.SelectedFilters);

        }

        [Fact(DisplayName = "FilterService - ApplyFilters - Pay - returns correct filter URL")]
        public void FilterService_ApplyFilters_Pay_Returns_Correct_Filter_URL()
        {

            IContentService contentService = A.Fake<IContentService>();

            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();

            ILogger<FilterService> filterServiceLogger = A.Fake<ILogger<FilterService>>();

            FilterService service = new FilterService(contentService, schemesModelService, filterServiceLogger);

            A.CallTo(() => contentService.Content).Returns(new GeneratedContent());

            HomeModel result = service.ApplyFilter(

                new SchemeFilterModel()

                {

                    Pay = new string[1] { "Pay" }

                }

            );

            Assert.Equal("pay=Pay", result.SelectedFilters);

        }

        [Fact(DisplayName = "FilterService - ApplyFilters - Duration - returns correct filter URL")]
        public void FilterService_ApplyFilters_Duration_Returns_Correct_Filter_URL()
        {

            IContentService contentService = A.Fake<IContentService>();

            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();

            ILogger<FilterService> filterServiceLogger = A.Fake<ILogger<FilterService>>();

            FilterService service = new FilterService(contentService, schemesModelService, filterServiceLogger);

            A.CallTo(() => contentService.Content).Returns(new GeneratedContent());

            HomeModel result = service.ApplyFilter(

                new SchemeFilterModel()

                {

                    SchemeLength = new string[1] { "Duration" }

                }

            );

            Assert.Equal("duration=Duration", result.SelectedFilters);

        }

        [Fact(DisplayName = "FilterService - ApplyFilters - Motivation - returns correct filter URL")]
        public void FilterService_ApplyFilters_Motivation_Returns_Correct_Filter_URL()
        {

            IContentService contentService = A.Fake<IContentService>();

            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();

            ILogger<FilterService> filterServiceLogger = A.Fake<ILogger<FilterService>>();

            FilterService service = new FilterService(contentService, schemesModelService, filterServiceLogger);

            A.CallTo(() => contentService.Content).Returns(new GeneratedContent());

            HomeModel result = service.ApplyFilter(

                new SchemeFilterModel()

                {

                    Motivations = new string[1] { "Motivation" }

                }

            );

            Assert.Equal("motivation=Motivation", result.SelectedFilters);

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
                    new SchemeFilterModel { SchemeLength = new[] { fourToTwelveMonths }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(yearOrMore)),
                    new SchemeFilterModel { SchemeLength = new[] { yearOrMore }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(unpaid)),
                    new SchemeFilterModel { Pay = new[] { unpaid }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(yearOrMore) && s.FilterAspects.Contains(unpaid)),
                    new SchemeFilterModel { SchemeLength = new[] { yearOrMore }, Pay = new[] { unpaid }}
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(fourToTwelveMonths) || s.FilterAspects.Contains(yearOrMore)),
                    new SchemeFilterModel { SchemeLength = new[] { fourToTwelveMonths, yearOrMore }}
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}