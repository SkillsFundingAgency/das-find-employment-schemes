using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.ViewModels
{
    public class SchemeFilterViewModelTests
    {
        [Theory]
        [ClassData(typeof(SchemeFilterViewModelTestData))]
        public void Constructor_AllFiltersCount(int expectedAllFiltersCount, SchemeFilterViewModel model)
        {
            Assert.Equal(expectedAllFiltersCount, model.AllFilters.Count());
        }

        public class SchemeFilterViewModelTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {  0, new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { }) };
                yield return new object[] {  4, new SchemeFilterViewModel(new string[] { "abc", "def" }, new string[] { "ghi" }, new string[] { "xyz" }) };
                yield return new object[] { 10, new SchemeFilterViewModel(new string[] { "abc", "def", "ghi" }, new string[] { "jk", "l", "mn" }, new string[] { "o", "pqrs", "tuvw", "xyz" }) };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}