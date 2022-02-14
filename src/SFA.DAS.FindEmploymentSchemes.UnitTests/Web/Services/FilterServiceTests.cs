using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class FilterServiceTests
    {
        [Theory]
        [ClassData(typeof(FilterServiceTestData))]
        public void ApplyFilters_Result(IEnumerable<Scheme> expectedSchemes, SchemeFilterViewModel filters)
        {
            IFilterService service = A.Fake<IFilterService>();
            HomeModel model = A.Fake<HomeModel>(x => x.WithArgumentsForConstructor(() => new HomeModel(null, expectedSchemes, null, false)));
            A.CallTo(() => service.ApplyFilter(filters)).Returns(model);

            HomeModel result = service.ApplyFilter(filters);
            HashSet<Scheme> expected = new HashSet<Scheme>(expectedSchemes);
            HashSet<Scheme> applied = new HashSet<Scheme>(result.Schemes);
            Assert.True(expected.SetEquals(applied));
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
                    new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { })
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(fourToTwelveMonths)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { fourToTwelveMonths }, new string[] { })
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(yearOrMore)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { yearOrMore }, new string[] { } )
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(unpaid)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { unpaid })
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(yearOrMore) && s.FilterAspects.Contains(unpaid)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { yearOrMore }, new string[] { unpaid })
                };
                yield return new object[] {
                    generatedContent.Schemes.Where(s => s.FilterAspects.Contains(fourToTwelveMonths) || s.FilterAspects.Contains(yearOrMore)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { fourToTwelveMonths, yearOrMore }, new string[] { })
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}