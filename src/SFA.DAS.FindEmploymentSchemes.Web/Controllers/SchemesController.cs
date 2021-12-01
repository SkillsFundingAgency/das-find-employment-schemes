using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class SchemesController : Controller
    {
        private readonly ILogger<SchemesController> _logger;

        private static readonly FilterGroupModel[] FilterGroupModels = new FilterGroupModel[]
        {
            new FilterGroupModel("motivations", "I want to", SchemesContent.MotivationsFilters),
            new FilterGroupModel("scheme-length", "Length of scheme?", SchemesContent.SchemeLengthFilters),
            new FilterGroupModel("pay", "I can offer", SchemesContent.PayFilters)
        };

        private static readonly HomeModel HomeModel = new HomeModel(SchemesContent.Schemes, FilterGroupModels);

        private static readonly IReadOnlyDictionary<string, SchemeDetailsModel> SchemeDetailsModels = BuildSchemeDetailsModelsDictionary();

        private static ReadOnlyDictionary<string, SchemeDetailsModel> BuildSchemeDetailsModelsDictionary()
        {
            var schemeDetailsModels = new Dictionary<string, SchemeDetailsModel>();

            foreach (string schemeUrl in SchemesContent.Schemes.Select(s => s.Url))
            {
                schemeDetailsModels.Add(schemeUrl, new SchemeDetailsModel(schemeUrl, SchemesContent.Schemes));
            }

            return new ReadOnlyDictionary<string, SchemeDetailsModel>(schemeDetailsModels);
        }

        public SchemesController(ILogger<SchemesController> logger)
        {
            _logger = logger;
        }

        public IActionResult Home()
        {
            return View(HomeModel);
        }

        public IActionResult Details(string schemeUrl)
        {
            if (!SchemeDetailsModels.TryGetValue(schemeUrl, out SchemeDetailsModel? schemeDetailsModel))
                return NotFound();

            return View(schemeDetailsModel);
        }
    }
}