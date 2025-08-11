using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class SchemesController : Controller
    {

        #region Properties

        private readonly ISchemesModelService _schemesModelService;

        private readonly IFilterService _filterService;

        #endregion

        #region Constructors

        public SchemesController(

            ISchemesModelService schemesModelService,

            IFilterService filterService
            
        )
        {

            _schemesModelService = schemesModelService;

            _filterService = filterService;

        }

        #endregion

        #region Scheme Home / Preview

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Home(string filters)
        {

            return View(
                
                await _filterService.RemapFilters(filters)
                
            );

        }

        // if we switched to post/redirect/get, we could cache the response, but hopefully the vast majority of our users will have javascript enabled
        [HttpPost]
        public async Task<IActionResult> Home(string actionButton, SchemeFilterModel filters)
        {

            if (actionButton == "Compare")
            {

                string aspects = string.Join(",", filters.FilterAspects);

                return RedirectToAction("Comparison", "Schemes", new { filters = aspects });

            } 

            HomeModel filteredModel = await _filterService.ApplyFilter(filters);

            return View("Home", filteredModel);

        }

        public async Task<IActionResult> HomePreview(string filters)
        {

            HomeModel model = await _filterService.RemapFilters(filters, true);

            return View(
                
                "home",

                model

            );

        }

        [HttpPost]
        public async Task<IActionResult> HomePreview(string actionButton, SchemeFilterModel filters)
        {

            if (actionButton == "Compare")
            {

                string aspects = string.Join(",", filters.FilterAspects);

                return RedirectToAction("ComparisonPreview", "Schemes", new { filters = aspects });

            }

            HomeModel filteredModel = await _filterService.ApplyFilter(filters, true);

            filteredModel.Preview = new PreviewModel(Enumerable.Empty<HtmlString>());

            return View("Home", filteredModel);

        }

        #endregion

        #region Scheme Details / Preview

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Details(string schemeUrl)
        {

            var schemeDetailsModel = _schemesModelService.GetSchemeDetailsModel(schemeUrl);

            if (schemeDetailsModel == null)
            {

                return RedirectToAction("PageNotFound", "Error");

            }

            return View(schemeDetailsModel);

        }

        public async Task<IActionResult> DetailsPreview(string schemeUrl)
        {

            var schemeDetailsModel = await _schemesModelService.GetSchemeDetailsModelPreview(schemeUrl);

            if (schemeDetailsModel == null)
            {

                return NotFound();

            }

            return View("Details", schemeDetailsModel);

        }

        #endregion

        #region Scheme Comparion / Preview

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public async Task<IActionResult> Comparison(string filters)
        {

            SchemeFilterModel schemeFilterModel = _filterService.CreateFilterModel(filters);

            HomeModel filteredModel = await _filterService.ApplyFilter(schemeFilterModel);

            ComparisonResultsModel resultsModel = _schemesModelService.CreateComparisonResultsModel(
                
                filteredModel.Schemes.Select(a => a.HtmlId),

                schemeFilterModel

            );

            return View("ComparisonResults", resultsModel);

        }

        public async Task<IActionResult> ComparisonPreview(string filters)
        {

            SchemeFilterModel schemeFilterModel = _filterService.CreateFilterModel(filters);

            HomeModel filteredModel = await _filterService.ApplyFilter(schemeFilterModel, true);

            ComparisonResultsModel resultsModel = await _schemesModelService.CreateComparisonResultsModelPreview(

                filteredModel.Schemes.Select(a => a.HtmlId),

                schemeFilterModel

            );

            return View("ComparisonResults", resultsModel);

        }

        #endregion

    }

}
