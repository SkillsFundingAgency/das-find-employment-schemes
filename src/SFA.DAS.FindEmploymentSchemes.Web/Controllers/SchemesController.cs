
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;


namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class SchemesController : Controller
    {
        private readonly ILogger<SchemesController> _logger;
        private readonly IFilterService _filterService;

        private readonly HomeModel? _homeModel = null;


        public SchemesController(ILogger<SchemesController> logger, IFilterService filterService)
        {
            _logger = logger;
            _filterService = filterService;
            _homeModel = new HomeModel(SchemesContent.Schemes, _filterService.FilterGroupModels());
        }

        public IActionResult Home()
        {
            return View(_filterService.HomeModel());
        }

        [HttpPost]
        public IActionResult Home(SchemeFilterViewModel filters)
        {
            return View(_filterService.ApplyFilter(filters));
        }

        public IActionResult Details(string schemeUrl)
        {
            if (!_filterService.SchemeDetailsModels().TryGetValue(schemeUrl, out SchemeDetailsModel? schemeDetailsModel))
                return NotFound();

            return View(schemeDetailsModel);
        }
    }
}
