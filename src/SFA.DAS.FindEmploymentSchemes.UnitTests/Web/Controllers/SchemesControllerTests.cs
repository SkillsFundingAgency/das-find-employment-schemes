using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;
using AutoFixture.Kernel;
using AutoFixture;
using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class SchemesControllerTests
    {
        [Theory]
        [ClassData(typeof(SchemesControllerTestsHomeTestData))]
        public void SchemesController_Home(IEnumerable<Scheme> expectedSchemes)
        {
            ILogger<SchemesController> logger = A.Fake<ILogger<SchemesController>>();
            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();
            A.CallTo(() => schemesModelService.HomeModel)
                .Returns(new HomeModel(null!, expectedSchemes, null!));

            IFilterService filterService = A.Fake<IFilterService>();

            SchemesController controller = new SchemesController(logger, schemesModelService, filterService);
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
            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();

            IFilterService filterService = A.Fake<IFilterService>();
            HomeModel expectedHomeModel = A.Fake<HomeModel>(x => x.WithArgumentsForConstructor(() => new HomeModel(null, expectedSchemes, null, false)));

            A.CallTo(() => filterService.ApplyFilter(filters))
             .Returns(expectedHomeModel);

            SchemesController controller = new SchemesController(logger, schemesModelService, filterService);
            Assert.NotNull(controller);

            IActionResult result = controller.Home(filters, "");
            Assert.True(result is ViewResult);
            ViewResult vr = (ViewResult)result;
            Assert.True(!(vr.Model is null) && vr.Model is HomeModel);

            HomeModel homeModel = (HomeModel)vr.Model;
            HashSet<Scheme> expected = new HashSet<Scheme>(expectedSchemes);
            HashSet<Scheme> fromView = new HashSet<Scheme>(homeModel.Schemes);
            Assert.True(expected.SetEquals(fromView));
        }

        [Fact]
        public void SchemesController_Details()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IHtmlContent),
                    typeof(HtmlString)));

            var schemes = fixture.CreateMany<Scheme>(5).ToArray();

            const int selectedScheme = 2;

            var schemeUrl = schemes[selectedScheme].Url;

            SchemeDetailsModel schemeDetailsModel = new SchemeDetailsModel(schemeUrl, schemes);

            ILogger<SchemesController> logger = A.Fake<ILogger<SchemesController>>();
            ISchemesModelService schemesModelService = A.Fake<ISchemesModelService>();
            A.CallTo(() => schemesModelService.GetSchemeDetailsModel(schemeUrl))
                .Returns(schemeDetailsModel);

            IFilterService filterService = A.Fake<IFilterService>();

            SchemesController controller = new SchemesController(logger, schemesModelService, filterService);
            Assert.NotNull(controller);

            IActionResult result = controller.Details(schemeUrl);
            Assert.True(result is ViewResult);
            ViewResult vr = (ViewResult)result;
            Assert.True(!(vr.Model is null) && vr.Model is SchemeDetailsModel);
        }
    }

    public class SchemesControllerTestsHomeTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new GeneratedContent().Schemes };
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
                    new GeneratedContent().Schemes,
                    new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { })
                };
            yield return new object[] {
                    new GeneratedContent().Schemes.Where(s => s.FilterAspects.Contains(fourToTwelveMonths)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { fourToTwelveMonths }, new string[] { })
                };
            yield return new object[] {
                    new GeneratedContent().Schemes.Where(s => s.FilterAspects.Contains(yearOrMore)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { yearOrMore }, new string[] { } )
                };
            yield return new object[] {
                    new GeneratedContent().Schemes.Where(s => s.FilterAspects.Contains(unpaid)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { }, new string[] { unpaid })
                };
            yield return new object[] {
                    new GeneratedContent().Schemes.Where(s => s.FilterAspects.Contains(yearOrMore) && s.FilterAspects.Contains(unpaid)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { yearOrMore }, new string[] { unpaid })
                };
            yield return new object[] {
                    new GeneratedContent().Schemes.Where(s => s.FilterAspects.Contains(fourToTwelveMonths) || s.FilterAspects.Contains(yearOrMore)),
                    new SchemeFilterViewModel(new string[] { }, new string[] { fourToTwelveMonths, yearOrMore }, new string[] { })
                };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
