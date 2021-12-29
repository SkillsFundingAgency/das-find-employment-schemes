
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;


namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class SchemesControllerTests
    {
        [Theory]
        [ClassData(typeof(SchemesControllerTestsHomeTestData))]
        public void SchemesController_Home(IEnumerable<Scheme> expectedSchemes, object dummy)
        {
            ILogger<SchemesController> logger = A.Fake<ILogger<SchemesController>>();
            IFilterService service = A.Fake<IFilterService>();
            A.CallTo(service)
             .Where(a => a.Method.Name.Equals("get_HomeModel"))
             .WithReturnType<HomeModel>()
             .ReturnsLazily(() => new HomeModel(expectedSchemes, null));

            SchemesController controller = new SchemesController(logger, service);
            Assert.NotNull(controller);

            IActionResult result = controller.Home();
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
            ILogger<SchemesController> logger = A.Fake<ILogger<SchemesController>>();
            IFilterService service = A.Fake<IFilterService>();
            HomeModel expectedHomeModel = A.Fake<HomeModel>(x=> x.WithArgumentsForConstructor(() => new HomeModel(expectedSchemes, null)));

            A.CallTo(() => service.ApplyFilter(filters))
             .Returns(expectedHomeModel);

            SchemesController controller = new SchemesController(logger, service);
            Assert.NotNull(controller);

            IActionResult result = controller.Home(filters);
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
            ILogger<SchemesController> logger = A.Fake<ILogger<SchemesController>>();
            IFilterService service = A.Fake<IFilterService>();
            Dictionary<string, SchemeDetailsModel> dictionary = new Dictionary<string, SchemeDetailsModel>();
            dictionary.Add(schemeUrl, expectedDetails);

            A.CallTo(service)
             .Where(a => a.Method.Name.Equals("get_SchemeDetailsModels"))
             .WithReturnType<IReadOnlyDictionary<string, SchemeDetailsModel>>()
             .ReturnsLazily(() => new ReadOnlyDictionary<string, SchemeDetailsModel>(dictionary));

            SchemesController controller = new SchemesController(logger, service);
            Assert.NotNull(controller);

            IActionResult result = controller.Details(schemeUrl);
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
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
