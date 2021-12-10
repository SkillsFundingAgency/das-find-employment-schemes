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
            return View(_filterService.HomeModel);
        }

#pragma warning disable SCS0016
        /// <summary>
        /// Filtering post back for when Javascript is disabled
        /// </summary>
        /// <remarks>
        /// * if we switched to post/redirect/get, we could cache the response, but hopefully the vast majority of our users will have javascript enabled.
        /// * we disable the CSRF security warning, as this post doesn't affect any state changes (there is also no site authentication).
        ///   if the above changes, then we'd have to enable core's antiforgery.
        /// </remarks>>
        [HttpPost]
        public IActionResult Home(SchemeFilterViewModel filters)
        {
            return View(_filterService.ApplyFilter(filters));
        }
#pragma warning restore SCS0016

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Details(string schemeUrl)
        {
            if (!_filterService.SchemeDetailsModels.TryGetValue(schemeUrl, out SchemeDetailsModel? schemeDetailsModel))
                return NotFound();

            return View(schemeDetailsModel);
        }
    }
}
