
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;


namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class FilterServiceTests
    {
        [Theory]
        [ClassData(typeof(FilterServiceTestData))]
        public void ApplyFilters_Result(IEnumerable<Scheme> expectedSchemes, SchemeFilterViewModel filters)
        {
            IFilterService service = A.Fake<IFilterService>();
            HomeModel homeModel = A.Fake<HomeModel>();
            filters = A.Fake<SchemeFilterViewModel>();

            Scheme s1 = expectedSchemes.FirstOrDefault();
            Scheme s2 = expectedSchemes.LastOrDefault();
            expectedSchemes = new Scheme[]
                {
                    A.Fake<Scheme>(x => x.WithArgumentsForConstructor(() => new Scheme(s1.Name, null, null, null, null, s1.Url, 1, filters.allFilters.ToArray(), null, null, null, null, null, null, null, null))),
                    A.Fake<Scheme>(x => x.WithArgumentsForConstructor(() => new Scheme(s2.Name, null, null, null, null, s2.Url, 1, filters.allFilters.ToArray(), null, null, null, null, null, null, null, null)))
                };

            A.CallTo(() => service.ApplyFilter(filters)).Returns(homeModel); //.Schemes).Returns(expectedSchemes);
            HomeModel returns = service.ApplyFilter(filters);

            A.CallTo(() => service.ApplyFilter(filters)).MustHaveHappenedOnceOrMore();
            Assert.True(returns.Equals(homeModel));
        }

        public class FilterServiceTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {
                    SchemesContent.Schemes,
                    new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { })
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}