using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.AspNetCore.Mvc;
using FakeItEasy;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Xunit;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class SchemesControllerTests
    {
        public Fixture Fixture { get; set; }
        public HomeModel HomeModel { get; set; }
        public ComparisonModel ComparisonModel { get; set; }
        public HomeModel PreviewHomeModel { get; set; }
        public ComparisonModel PreviewComparisonModel { get; set; }
        public ISchemesModelService SchemesModelService { get; set; }
        public IFilterService FilterService { get; set; }
        public SchemeFilterModel SchemeFilterModel { get; set; }

        public SchemesController SchemesController { get; set; }

        public SchemesControllerTests()
        {
            Fixture = new Fixture();
            Fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IHtmlContent),
                    typeof(HtmlString)));

            FilterService = A.Fake<IFilterService>();
            SchemesModelService = A.Fake<ISchemesModelService>();

            HomeModel = new HomeModel(Fixture.Create<IHtmlContent>(), null!, null!);
            PreviewHomeModel = new HomeModel(Fixture.Create<IHtmlContent>(), null!, null!);

            ComparisonModel = new ComparisonModel(null!);
            PreviewComparisonModel = new ComparisonModel(null!);

            A.CallTo(() => SchemesModelService.HomeModel)
                .Returns(HomeModel);

            A.CallTo(() => SchemesModelService.CreateHomeModelPreview())
                .Returns(PreviewHomeModel);

            A.CallTo(() => SchemesModelService.ComparisonModel)
                .Returns(ComparisonModel);

            A.CallTo(() => SchemesModelService.CreateComparisonModelPreview())
                .Returns(PreviewComparisonModel);

            SchemesController = new SchemesController(SchemesModelService, FilterService);

            SchemeFilterModel = new SchemeFilterModel();
        }

        [Fact]
        public void Home_DefaultViewTest()
        {
            // act
            IActionResult result = SchemesController.Home();

            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void Home_HomeModelIsUsedTest()
        {
            // act
            IActionResult result = SchemesController.Home();

            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.NotNull(viewResult.Model);
            Assert.IsType<HomeModel>(viewResult.Model);
            Assert.Equal(HomeModel, viewResult.Model);
        }

        [Fact]
        public void PostHome_FilteredHomeModelIsUsedTest()
        {
            var filteredHomeModel = new HomeModel(null!, null!, null!);

            A.CallTo(() => FilterService.ApplyFilter(SchemeFilterModel))
                .Returns(filteredHomeModel);

            // act
            IActionResult result = SchemesController.Home(SchemeFilterModel, "");

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.IsType<HomeModel>(viewResult.Model);
            Assert.Equal(filteredHomeModel, viewResult.Model);
        }

        //todo: change cut to return Redirect object instead
#if Too_much_interals_set_up_for_value
        [Fact]
        public void PostHome_ShowFilterRedirectsToHomeTest()
        {
            var httpContext = A.Fake<HttpContext>();
            var httpResponse = A.Fake<HttpResponse>();

            A.CallTo(() => httpContext.Response)
                .Returns(httpResponse);

            SchemesController.ControllerContext = new ControllerContext(new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor()));

            // act
            SchemesController.Home(SchemeFilterModel, "filter");

            A.CallTo(() => httpResponse.Redirect("/"))
                .MustHaveHappenedOnceExactly();
        }
#endif

        [Fact]
        public void Details_KnownSchemeUrlReturnsViewResultWithDefaultViewTest()
        {
            var schemes = Fixture.CreateMany<Scheme>(2).ToArray();

            string schemeUrl = schemes.First().Url;

            var schemeDetailsModel = new SchemeDetailsModel(schemeUrl, schemes);

            A.CallTo(() => SchemesModelService.GetSchemeDetailsModel(schemeUrl))
                .Returns(schemeDetailsModel);

            // act
            IActionResult result = SchemesController.Details(schemeUrl);

            Assert.IsNotType<NotFoundResult>(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void Details_KnownSchemeUrlReturnsViewWithCorrectModelTest()
        {
            var schemes = Fixture.CreateMany<Scheme>(2).ToArray();

            string schemeUrl = schemes.First().Url;

            var schemeDetailsModel = new SchemeDetailsModel(schemeUrl, schemes);

            A.CallTo(() => SchemesModelService.GetSchemeDetailsModel(schemeUrl))
                .Returns(schemeDetailsModel);

            // act
            IActionResult result = SchemesController.Details(schemeUrl);

            Assert.IsNotType<NotFoundResult>(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal(schemeDetailsModel, viewResult.Model);
        }

        [Fact]
        public void Details_UnknownSchemeUrlReturnsNotFoundTest()
        {
            const string schemeUrl = "unknown-scheme";

            A.CallTo(() => SchemesModelService.GetSchemeDetailsModel(schemeUrl))
                .Returns(null);

            // act
            IActionResult result = SchemesController.Details(schemeUrl);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Comparison_ModelTest()
        {
            // act
            IActionResult result = SchemesController.Comparison();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(ComparisonModel, viewResult.Model);
        }

        [Fact]
        public void Comparison_DefaultViewTest()
        {
            // act
            IActionResult result = SchemesController.Comparison();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async Task ComparisonPreview_ModelTest()
        {
            // act
            IActionResult result = await SchemesController.ComparisonPreview();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(PreviewComparisonModel, viewResult.Model);
        }

        [Fact]
        public async Task ComparisonPreview_ComparisonViewTest()
        {
            // act
            IActionResult result = await SchemesController.ComparisonPreview();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Comparison", viewResult.ViewName);
        }

        // duplicate - should be testing PostComparisonPreview
        //[Fact]
        //public async Task ComparisonPreview_ModelTest()
        //{
        //    var schemes = Fixture.CreateMany<Scheme>(2).ToArray();

        //    PreviewComparisonModel = new ComparisonModel(schemes);

        //    A.CallTo(() => SchemesModelService.CreateComparisonModelPreview())
        //        .Returns(PreviewComparisonModel);

        //    // act
        //    IActionResult result = await SchemesController.ComparisonPreview();

        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    Assert.Equal(PreviewComparisonModel, viewResult.Model);
        //}

        [Fact]
        public async Task PostComparisonPreview_ComparisonResultsViewTest()
        {
            var schemes = Fixture.CreateMany<Scheme>(2).ToArray();

            PreviewHomeModel = new HomeModel(null, schemes, null);

            A.CallTo(() => SchemesModelService.CreateHomeModelPreview())
                .Returns(PreviewHomeModel);

            // act
            IActionResult result = await SchemesController.ComparisonPreview(Array.Empty<string>());

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ComparisonResults", viewResult.ViewName);
        }
    }
}
