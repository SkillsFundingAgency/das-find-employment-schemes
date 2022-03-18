using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.ViewModels
{
    public class SchemeFilterViewModelTests
    {
        [Theory]
        [ClassData(typeof(SchemeFilterViewModelTestData))]
        public void Constructor_AllFiltersCount(int expectedAllFiltersCount, SchemeFilterModel model)
        {
            Assert.Equal(expectedAllFiltersCount, model.AllFilters.Count());
        }

        public class SchemeFilterViewModelTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {  0, new SchemeFilterModel() };
                yield return new object[] {  4, new SchemeFilterModel { Motivations = new[] { "abc", "def" }, SchemeLength = new[] { "ghi" }, Pay = new[] { "xyz" }}};
                yield return new object[] { 10, new SchemeFilterModel { Motivations = new[] { "abc", "def", "ghi" }, SchemeLength = new[] { "jk", "l", "mn" }, Pay = new[] { "o", "pqrs", "tuvw", "xyz" }}};
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}