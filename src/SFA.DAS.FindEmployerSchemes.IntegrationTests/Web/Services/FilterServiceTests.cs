using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;

namespace SFA.DAS.FindEmploymentSchemes.IntegrationTests.Web.Services
{
    public class FilterServiceTests
    {
        private readonly IServiceProvider _services = Program.GetServices();
        private IFilterService _service;

        [Theory]
        [ClassData(typeof(FilterServiceTestData))]
        public void ApplyFilters_Result(IEnumerable<Scheme> expectedSchemes, SchemeFilterViewModel filters)
        {
            _service = _services.GetRequiredService<IFilterService>();
            Assert.NotNull(_service);
            
            HashSet<Scheme> expected = new HashSet<Scheme>(expectedSchemes);
            HashSet<Scheme> applied = new HashSet<Scheme>(_service.ApplyFilter(filters).Schemes);
            Assert.True(expected.SetEquals(applied));
        }

        public class FilterServiceTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                string fourToTwelveMonths = "scheme-length--4-months-to-12-months";
                string yearOrMore = "scheme-length--a-year-or-more";
                string unpaid = "pay--unpaid";

                yield return new object[] {
                    SchemesContent.Schemes,
                    new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { })
                };
                yield return new object[] {
                    SchemesContent.Schemes.Where(s => s.FilterAspects.Contains(fourToTwelveMonths)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { fourToTwelveMonths }, new string[] { })
                };
                yield return new object[] {
                    SchemesContent.Schemes.Where(s => s.FilterAspects.Contains(yearOrMore)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { yearOrMore }, new string[] { } )
                };
                yield return new object[] {
                    SchemesContent.Schemes.Where(s => s.FilterAspects.Contains(unpaid)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { unpaid })
                };
                yield return new object[] {
                    SchemesContent.Schemes.Where(s => s.FilterAspects.Contains(yearOrMore) && s.FilterAspects.Contains(unpaid)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { yearOrMore }, new string[] { unpaid })
                };
                yield return new object[] {
                    SchemesContent.Schemes.Where(s => s.FilterAspects.Contains(fourToTwelveMonths) || s.FilterAspects.Contains(yearOrMore)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { fourToTwelveMonths, yearOrMore }, new string[] { })
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}