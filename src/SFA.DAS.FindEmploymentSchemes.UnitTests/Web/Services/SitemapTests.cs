
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FakeItEasy;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Extensions;


namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Services
{
    public class SitemapTests
    {
        [Theory]
        [ClassData(typeof(SitemapTestData))]
        public void Sitemap_Generate(string dummy, string baseUrl)
        {
            IWebHostEnvironment env = A.Fake<IWebHostEnvironment>();
            IConfiguration config = A.Fake<IConfiguration>();
            Uri uri = A.Fake<Uri>(x => x.Wrapping(new Uri(baseUrl)));
            IServiceCollection services = A.Fake<IServiceCollection>();

            IServiceCollection postCallServices = services.GenerateSitemap(config, env);
            Assert.True(postCallServices.Equals(services));
        }
    }

    public class SitemapTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, "https://a-domain-some-where.education.gov.uk/" };
            yield return new object[] { null, "https://another-domain-some-where.education.gov.uk/" };
            yield return new object[] { null, "https://a-domain-some-where-else.education.gov.uk/" };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
