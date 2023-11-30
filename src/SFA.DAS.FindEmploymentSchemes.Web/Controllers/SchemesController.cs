using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Home(string pay, string duration, string motivation)
        {

            return View(
                
                _filterService.RemapFilters(pay, duration, motivation)
                
            );

        }

        // if we switched to post/redirect/get, we could cache the response, but hopefully the vast majority of our users will have javascript enabled
        [HttpPost]
        public IActionResult Home(string actionButton, SchemeFilterModel filters)
        {
            
            if (actionButton == "Compare")
            {

                string pay = string.Join(",", filters.Pay);

                string duration = string.Join(",", filters.SchemeLength);

                string motivation = string.Join(",", filters.Motivations);

                return RedirectToAction("Comparison", "Schemes", new { pay, duration, motivation });

            } 

            HomeModel filteredModel = _filterService.ApplyFilter(filters);

            return View("Home", filteredModel);

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

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Comparison(string pay, string duration, string motivation)
        {

            SchemeFilterModel schemeFilterModel = _filterService.CreateFilterModel(pay, duration, motivation);

            HomeModel filteredModel = _filterService.ApplyFilter(schemeFilterModel);

            ComparisonResultsModel resultsModel = _schemesModelService.CreateComparisonResultsModel(
                
                filteredModel.Schemes.Select(a => a.HtmlId),

                schemeFilterModel

            );

            return View("ComparisonResults", resultsModel);

        }

        public async Task<IActionResult> ComparisonPreview(string pay, string duration, string motivation)
        {

            SchemeFilterModel schemeFilterModel = _filterService.CreateFilterModel(pay, duration, motivation);

            HomeModel filteredModel = _filterService.ApplyFilter(schemeFilterModel);

            ComparisonResultsModel resultsModel = await _schemesModelService.CreateComparisonResultsModelPreview(

                filteredModel.Schemes.Select(a => a.HtmlId),

                schemeFilterModel

            );

            return View("ComparisonResults", resultsModel);

        }
    }
}
