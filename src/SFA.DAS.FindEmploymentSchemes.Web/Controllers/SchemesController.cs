using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class SchemesController : Controller
    {
        private readonly ISchemesModelService _schemesModelService;
        private readonly IFilterService _filterService;

        public SchemesController(
            ISchemesModelService schemesModelService,
            IFilterService filterService)
        {
            _schemesModelService = schemesModelService;
            _filterService = filterService;
        }

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Home()
        {
            return View(_schemesModelService.HomeModel);
        }

        // if we switched to post/redirect/get, we could cache the response, but hopefully the vast majority of our users will have javascript enabled
        [HttpPost]
        public IActionResult Home(SchemeFilterModel filters, [FromQuery] string? show)
        {
            if (show is "filter")
            {
                // handle the case where we are on mobile, javascript is disabled, the user has performed a filter, then clicked 'filter schemes'.
                // the user loses their current set of selected filters, but this is such an edge case they'll have to live with it,
                // as a penance for disabling javascript on their phone's browser (it's not even possible on mobile chrome!)
                // they can still perform another filter.
                Response.Redirect("/");
            }

            return View(_filterService.ApplyFilter(filters));
        }

        public async Task<IActionResult> HomePreview()
        {
            return View("home", await _schemesModelService.CreateHomeModelPreview());
        }

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Details(string schemeUrl)
        {
            var schemeDetailsModel = _schemesModelService.GetSchemeDetailsModel(schemeUrl);
            if (schemeDetailsModel == null)
                return NotFound();

            return View(schemeDetailsModel);
        }

        public async Task<IActionResult> DetailsPreview(string schemeUrl)
        {
            var schemeDetailsModel = await _schemesModelService.GetSchemeDetailsModelPreview(schemeUrl);
            if (schemeDetailsModel == null)
                return NotFound();

            return View("Details", schemeDetailsModel);
        }

        public IActionResult Comparison()
        {
            return View(_schemesModelService.ComparisonModel);
        }

        [HttpPost]
        public IActionResult Comparison(string[] schemes)
        {
            if (schemes.Any())
            {
                ComparisonResultsModel resultsModel = _schemesModelService.CreateComparisonResultsModel(schemes);
                return View("ComparisonResults", resultsModel);
            }

            ComparisonModel model = new ComparisonModel(_schemesModelService.ComparisonModel.Schemes, true);
            return View(model);
        }

        public async Task<IActionResult> ComparisonPreview()
        {
            var comparisonModel = await _schemesModelService.CreateComparisonModelPreview();
            return View("Comparison", comparisonModel);
        }

        [HttpPost]
        public async Task<IActionResult> ComparisonPreview(string[] schemes)
        {
            var model = await _schemesModelService.CreateComparisonResultsModelPreview(schemes);
            return View("ComparisonResults", model);
        }
    }
}
