
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;


namespace SFA.DAS.FindEmploymentSchemes.IntegrationTests.Web.Controllers
{
    public class SchemesControllerTests
    {
        private readonly IServiceProvider _services = Program.GetServices();
        private ILogger<SchemesController> _logger;
        private IFilterService _service;
        private SchemesController _controller = null;

        [Theory]
        [ClassData(typeof(SchemesControllerTestsHomeTestData))]
        public void SchemesController_Home(IEnumerable<Scheme> expectedSchemes, object dummy)
        {
            _logger = _services.GetRequiredService<ILogger<SchemesController>>();
            _service = _services.GetRequiredService<IFilterService>();
            Assert.NotNull(_logger);
            _controller = new SchemesController(_logger, _service);
            Assert.NotNull(_controller);

            IActionResult result = _controller.Home();
            Assert.True(result is ViewResult);
            ViewResult vr = (ViewResult)result;
            Assert.True(!(vr.Model is null) && vr.Model is HomeModel);

            HomeModel homeModel = (HomeModel)vr.Model;
            HashSet<Scheme> expected = new HashSet<Scheme>(expectedSchemes);
            HashSet<Scheme> fromView = new HashSet<Scheme>(homeModel.Schemes);
            Assert.True(expected.SetEquals(fromView));
        }

        [Theory]
        [ClassData(typeof(SchemesControllerTestsFilteredHomeTestData))]
        public void SchemesController_FilteredHome(IEnumerable<Scheme> expectedSchemes, SchemeFilterViewModel filters)
        {
            _logger = _services.GetRequiredService<ILogger<SchemesController>>();
            _service = _services.GetRequiredService<IFilterService>();
            Assert.NotNull(_logger);
            Assert.NotNull(_service);
            _controller = new SchemesController(_logger, _service);
            Assert.NotNull(_controller);

            IActionResult result = _controller.Home(filters);
            Assert.True(result is ViewResult);
            ViewResult vr = (ViewResult)result;
            Assert.True(!(vr.Model is null) && vr.Model is HomeModel);

            HomeModel homeModel = (HomeModel)vr.Model;
            HashSet<Scheme> expected = new HashSet<Scheme>(expectedSchemes);
            HashSet<Scheme> fromView = new HashSet<Scheme>(homeModel.Schemes);
            Assert.True(expected.SetEquals(fromView));
        }

        [Theory]
        [ClassData(typeof(SchemesControllerTestsDetailsTestData))]
        public void SchemesController_Details(SchemeDetailsModel expectedDetails, string schemeUrl)
        {
            _logger = _services.GetRequiredService<ILogger<SchemesController>>();
            _service = _services.GetRequiredService<IFilterService>();
            Assert.NotNull(_logger);
            Assert.NotNull(_service);
            _controller = new SchemesController(_logger, _service);
            Assert.NotNull(_controller);

            IActionResult result = _controller.Details(schemeUrl);
            Assert.True(result is ViewResult);
            ViewResult vr = (ViewResult)result;
            Assert.True(!(vr.Model is null) && vr.Model is SchemeDetailsModel);

            SchemeDetailsModel detailsModel = (SchemeDetailsModel)vr.Model;
            Assert.True(expectedDetails.Scheme.Name == detailsModel.Scheme.Name);
        }
    }

    public class SchemesControllerTestsHomeTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() {
            yield return new object[] { SchemesContent.Schemes, null };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class SchemesControllerTestsFilteredHomeTestData : IEnumerable<object[]>
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

    public class SchemesControllerTestsDetailsTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator() {
            foreach(Scheme S in SchemesContent.Schemes) {
                yield return new object[] { new SchemeDetailsModel(S.Url, SchemesContent.Schemes), S.Url };
            }

            //yield return new object[] { SchemesContent.Schemes.FirstOrDefault(p => p.Url == "apprenticeships"), "apprenticeships" };
            //yield return new object[] { SchemesContent.Schemes.FirstOrDefault(p => p.Url == "t-levels-industry-placements"), "t-levels-industry-placements" };
            //yield return new object[] { SchemesContent.Schemes.FirstOrDefault(p => p.Url == "sector-based-work-academy-programme-swap"), "sector-based-work-academy-programme-swap" };
            //yield return new object[] { SchemesContent.Schemes.FirstOrDefault(p => p.Url == "skills-bootcamps"), "skills-bootcamps" };
            //yield return new object[] { SchemesContent.Schemes.FirstOrDefault(p => p.Url == "cookies"), "cookies" };
            //yield return new object[] { SchemesContent.Schemes.FirstOrDefault(p => p.Url == "cookies"), "cookies" };
            //yield return new object[] { SchemesContent.Schemes.FirstOrDefault(p => p.Url == "cookies"), "cookies" };
            //yield return new object[] { SchemesContent.Schemes.FirstOrDefault(p => p.Url == "cookies"), "cookies" };
            //yield return new object[] { SchemesContent.Schemes.FirstOrDefault(p => p.Url == "cookies"), "cookies" };
            //yield return new object[] { SchemesContent.Schemes.FirstOrDefault(p => p.Url == "cookies"), "cookies" };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
