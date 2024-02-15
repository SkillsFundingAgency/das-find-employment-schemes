using AutoFixture;
using AutoFixture.Kernel;
using Contentful.Core.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Controllers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SFA.DAS.FindEmploymentSchemes.UnitTests.Web.Controllers
{
    public class SchemesControllerTests
    {
        public Fixture Fixture { get; set; }
        public HomeModel HomeModel { get; set; }
        public ComparisonModel ComparisonModel { get; set; }
        public HomeModel PreviewHomeModel { get; set; }
        public ComparisonModel PreviewComparisonModel { get; set; }
        public ComparisonResultsModel ComparisonResultsModel { get; set; }
        public ISchemesModelService SchemesModelService { get; set; }
        public IFilterService FilterService { get; set; }
        public SchemeFilterModel SchemeFilterModel { get; set; }

        public SchemesController SchemesController { get; set; }

        private BetaBanner BetaBanner { get; set; }

        private SchemeComparison SchemeComparison { get; set; }

        public SchemesControllerTests()
        {

            BetaBanner = new BetaBanner() { BetaBannerID = "BetaBannerID", BetaBannerTitle = "BetaBannerTitle", BetaBannerContent = null };

            SchemeComparison = new SchemeComparison() 
            { 
            
                SchemeComparisonTitle = "SchemeComparisonTitle",

                SchemeComparisonActionButtonText = "SchemeComparisonActionButtonText",

                SchemeComparisonAgeHeading = "SchemeComparisonAgeHeading",

                SchemeComparisonCostHeading = "SchemeComparisonCostHeading",

                SchemeComparisonDurationHeading = "SchemeComparisonDurationHeading",

                SchemeComparisonRecruitOrTrainHeading = "SchemeComparisonRecruitOrTrainHeading",

                SchemeComparisonTitleColumnHeading = "SchemeComparisonTitleColumnHeading"

            };

            Fixture = new Fixture();

            Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()

                .ForEach(b => Fixture.Behaviors.Remove(b));

            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            Fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IHtmlContent),
                    typeof(HtmlString)));

            Fixture.Customizations.Add(
            new TypeRelay(
                typeof(IContent),
                typeof(Document)));

            FilterService = A.Fake<IFilterService>();
            SchemesModelService = A.Fake<ISchemesModelService>();

            HomeModel = new HomeModel(Fixture.Create<IHtmlContent>(), null!, null!, [], BetaBanner);

            PreviewHomeModel = new HomeModel(Fixture.Create<IHtmlContent>(), null!, null!, [], BetaBanner);

            A.CallTo(() => SchemesModelService.HomeModel)
                .Returns(HomeModel);

            A.CallTo(() => SchemesModelService.CreateHomeModelPreview())
                .Returns(PreviewHomeModel);

            SchemesController = new SchemesController(SchemesModelService, FilterService);

            SchemeFilterModel = new SchemeFilterModel();
        }

        [Fact]
        public void Home_DefaultViewTest()
        {
            // act
            IActionResult result = SchemesController.Home(string.Empty);

            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void Home_HomeModelIsUsedTest()
        {
            // act
            IActionResult result = SchemesController.Home(string.Empty);

            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.NotNull(viewResult.Model);
            Assert.IsAssignableFrom<HomeModel>(viewResult.Model);
        }

        [Fact]
        public void PostHome_FilteredHomeModelIsUsedTest()
        {
            var filteredHomeModel = new HomeModel(null!, null!, null!, [], BetaBanner);

            A.CallTo(() => FilterService.ApplyFilter(SchemeFilterModel))
                .Returns(filteredHomeModel);

            // act
            IActionResult result = SchemesController.Home("HomeAction", SchemeFilterModel);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.IsType<HomeModel>(viewResult.Model);
            Assert.Equal(filteredHomeModel, viewResult.Model);
        }

        [Fact]
        public async Task HomePreview_HomeViewTest()
        {
            // act
            IActionResult result = await SchemesController.HomePreview();

            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal("home", viewResult.ViewName);
        }

        [Fact]
        public async Task HomePreview_HomeModelIsUsedTest()
        {
            // act
            IActionResult result = await SchemesController.HomePreview();

            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.NotNull(viewResult.Model);
            Assert.IsType<HomeModel>(viewResult.Model);
            Assert.Equal(PreviewHomeModel, viewResult.Model);
        }

        [Fact]
        public void Details_KnownSchemeUrlReturnsViewResultWithDefaultViewTest()
        {
            var schemes = Fixture.CreateMany<Scheme>(2).ToArray();

            string schemeUrl = schemes.First().Url;

            var schemeDetailsModel = new SchemeDetailsModel(schemeUrl, schemes, [], BetaBanner);

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

            var schemeDetailsModel = new SchemeDetailsModel(schemeUrl, schemes, [], BetaBanner);

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
        public async Task DetailsPreview_KnownSchemeUrlReturnsViewResultWithDetailsViewTest()
        {
            var schemes = Fixture.CreateMany<Scheme>(2).ToArray();

            string schemeUrl = schemes.First().Url;

            var schemeDetailsModel = new SchemeDetailsModel(schemeUrl, schemes, [], BetaBanner);

            A.CallTo(() => SchemesModelService.GetSchemeDetailsModelPreview(schemeUrl))
                .Returns(schemeDetailsModel);

            // act
            IActionResult result = await SchemesController.DetailsPreview(schemeUrl);

            Assert.IsNotType<NotFoundResult>(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Details", viewResult.ViewName);
        }

        [Fact]
        public async Task DetailsPreview_KnownSchemeUrlReturnsViewWithCorrectModelTest()
        {
            var schemes = Fixture.CreateMany<Scheme>(2).ToArray();

            string schemeUrl = schemes.First().Url;

            var schemeDetailsModel = new SchemeDetailsModel(schemeUrl, schemes, [], BetaBanner);

            A.CallTo(() => SchemesModelService.GetSchemeDetailsModelPreview(schemeUrl))
                .Returns(schemeDetailsModel);

            // act
            IActionResult result = await SchemesController.DetailsPreview(schemeUrl);

            Assert.IsNotType<NotFoundResult>(result);
            Assert.IsType<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.Equal(schemeDetailsModel, viewResult.Model);
        }

        [Fact]
        public async Task DetailsPreview_UnknownSchemeUrlReturnsNotFoundTest()
        {
            const string schemeUrl = "unknown-scheme";

            A.CallTo(() => SchemesModelService.GetSchemeDetailsModelPreview(schemeUrl))
                .Returns(default (SchemeDetailsModel));

            // act
            IActionResult result = await SchemesController.DetailsPreview(schemeUrl);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Comparison_ReturnsViewWithComparisonResultsModel()
        {

            var filterServiceMock = A.Fake<IFilterService>();

            var schemesModelServiceMock = A.Fake<ISchemesModelService>();

            var controller = new SchemesController(schemesModelServiceMock, filterServiceMock);

            // Mock data

            string filters = "filters";

            // Mock SchemeFilterModel and HomeModel

            var schemeFilterModel = new SchemeFilterModel
            {

            };

            // Mock ApplyFilter method

            A.CallTo(() => FilterService.ApplyFilter(schemeFilterModel))
                .Returns(PreviewHomeModel);

            // Mock CreateComparisonResultsModel method

            A.CallTo(() => schemesModelServiceMock.CreateComparisonResultsModel(new List<string>(), schemeFilterModel))
                .Returns(new ComparisonResultsModel(SchemeComparison, new List<Scheme>(), schemeFilterModel, [], BetaBanner));

            // Act
            var result = controller.Comparison(filters) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ComparisonResults", result.ViewName);
            Assert.IsAssignableFrom<ComparisonResultsModel>(result.Model);
        }

        [Fact]
        public async Task ComparisonPreview_ReturnsViewWithComparisonResultsModel()
        {

            var filterServiceMock = A.Fake<IFilterService>();

            var schemesModelServiceMock = A.Fake<ISchemesModelService>();

            var controller = new SchemesController(schemesModelServiceMock, filterServiceMock);

            // Mock data

            var filters = "filters";

            // Mock SchemeFilterModel and HomeModel

            var schemeFilterModel = new SchemeFilterModel
            {

            };

            // Mock ApplyFilter method

            A.CallTo(() => FilterService.ApplyFilter(schemeFilterModel))
                .Returns(PreviewHomeModel);

            // Mock CreateComparisonResultsModelPreview method

            A.CallTo(() => schemesModelServiceMock.CreateComparisonResultsModelPreview(new List<string>(), schemeFilterModel))
                .Returns(new ComparisonResultsModel(SchemeComparison, new List<Scheme>(), schemeFilterModel, [], BetaBanner));

            // Act
            var result = await controller.ComparisonPreview(filters) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ComparisonResults", result.ViewName);
            Assert.IsAssignableFrom<ComparisonResultsModel>(result.Model);
        }

    }

}
