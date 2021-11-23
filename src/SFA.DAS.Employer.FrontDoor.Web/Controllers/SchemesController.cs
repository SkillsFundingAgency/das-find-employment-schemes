using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Employer.FrontDoor.Web.Content;
using SFA.DAS.Employer.FrontDoor.Web.Models;

namespace SFA.DAS.Employer.FrontDoor.Web.Controllers
{
    public class SchemesController : Controller
    {
        private readonly ILogger<SchemesController> _logger;

        private static readonly HomeModel HomeModel = new HomeModel(SchemesContent.Schemes);

        private static readonly IReadOnlyDictionary<string, SchemeDetailsModel> SchemeDetailsModels = BuildSchemeDetailsModelsDictionary();

        static ReadOnlyDictionary<string, SchemeDetailsModel> BuildSchemeDetailsModelsDictionary()
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
