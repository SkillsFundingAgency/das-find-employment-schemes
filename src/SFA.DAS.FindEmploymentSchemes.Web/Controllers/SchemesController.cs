
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Web.Infrastructure;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;


namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class SchemesController : Controller
    {
        private readonly ILogger<SchemesController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IFilterService _filterService;

        public SchemesController(ILogger<SchemesController> logger,
                                 IWebHostEnvironment environment,
                                 IFilterService filterService)
        {
            _logger = logger;
            _environment = environment;
            _filterService = filterService;
        }

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Home()
        {
            Sitemap.Generate(_environment, new System.Uri($"{Request.Scheme}://{Request.Host}"));
            return View(_filterService.HomeModel);
        }

        // if we switched to post/redirect/get, we could cache the response, but hopefully the vast majority of our users will have javascript enabled
        [HttpPost]
        public IActionResult Home(SchemeFilterViewModel filters, [FromQuery] string? show)
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

        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Details(string schemeUrl)
        {
            if (!_filterService.SchemeDetailsModels.TryGetValue(schemeUrl, out SchemeDetailsModel? schemeDetailsModel))
                return NotFound();

            return View(schemeDetailsModel);
        }
    }
}
