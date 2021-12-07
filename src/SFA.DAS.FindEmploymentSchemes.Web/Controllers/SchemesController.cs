using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class SchemesController : Controller
    {
        private readonly ILogger<SchemesController> _logger;
        private readonly IFilterService _filterService;

        public SchemesController(ILogger<SchemesController> logger, IFilterService filterService)
        {
            _logger = logger;
            _filterService = filterService;
        }

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Home()
        {
            return View(_filterService.HomeModel());
        }

        [HttpPost]
        public IActionResult Home(SchemeFilterViewModel filters)
        {
            return View(_filterService.ApplyFilter(filters));
        }

        //todo: if params are in query, can vary by them
        public IActionResult Details(string schemeUrl)
        {
            if (!_filterService.SchemeDetailsModels().TryGetValue(schemeUrl, out SchemeDetailsModel? schemeDetailsModel))
                return NotFound();

            return View(schemeDetailsModel);
        }
    }
}
