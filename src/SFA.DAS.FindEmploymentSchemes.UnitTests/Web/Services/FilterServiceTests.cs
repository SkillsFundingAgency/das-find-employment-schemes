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

            FilterService service = new FilterService(contentService, schemesModelService);
            HomeModel model = A.Fake<HomeModel>(x => x.WithArgumentsForConstructor(() => new HomeModel(null, expectedSchemes, null, false)));
            A.CallTo(() => contentService.Content).Returns(new GeneratedContent());

            HomeModel result = service.ApplyFilter(filters);
            Assert.True(expectedSchemes.Count() == result.Schemes.Count());
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