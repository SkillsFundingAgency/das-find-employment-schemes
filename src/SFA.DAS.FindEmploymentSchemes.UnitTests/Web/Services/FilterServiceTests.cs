
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;


namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class FilterServiceTests
    {
        private readonly IServiceProvider _services = Program.CreateHostBuilder(new string[] { }).Build().Services;
        private IFilterService _service;

        [Theory]
        [ClassData(typeof(FilterServiceTestData))]
        public void ApplyFilters_Result(IEnumerable<Scheme> expectedSchemes, SchemeFilterViewModel filters)
        {
            _service = _services.GetRequiredService<IFilterService>();
            
            HashSet<Scheme> expected = new HashSet<Scheme>(expectedSchemes);
            HashSet<Scheme> applied = new HashSet<Scheme>(_service.ApplyFilter(filters).Schemes);
            Assert.True(expected.SetEquals(applied));
        }

        public class FilterServiceTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { new Scheme[] { }, new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { } ) };
                yield return new object[] { new Scheme[] { }, new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { }) };
                yield return new object[] { new Scheme[] { }, new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { }) };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}