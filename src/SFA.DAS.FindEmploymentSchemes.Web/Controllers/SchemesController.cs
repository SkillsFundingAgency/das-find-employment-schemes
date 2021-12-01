
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;


namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class SchemesController : Controller
    {
        private readonly ILogger<SchemesController> _logger;

        private static readonly FilterGroupModel[] FilterGroupModels = new FilterGroupModel[]
        {
            new FilterGroupModel("motivations", "I want to", SchemesContent.MotivationsFilters),
            new FilterGroupModel("schemeLength", "Length of scheme?", SchemesContent.SchemeLengthFilters),
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

        [HttpPost]
        public IActionResult Home(SchemeFilterViewModel filters)
        {
            IEnumerable<Scheme> filteredSchemes = from Scheme s in HomeModel.Schemes
                                                  from string f in filters.allFilters
                                                  where s.FilterAspects.Contains(f)
                                                  select s;
            filteredSchemes = filteredSchemes.Distinct();
            if (!filters.allFilters.Any())
                filteredSchemes = HomeModel.Schemes;

            List<FilterGroupModel> filterGroupModels = new List<FilterGroupModel> { };
            filterGroupModels.Add(new FilterGroupModel("motivations", "I want to",
                                  SchemesContent.MotivationsFilters.Select(x => new MotivationsFilter(x.Id, x.Description, filters.motivations.Contains(x.Id)))));
            filterGroupModels.Add(new FilterGroupModel("schemeLength", "Length of scheme?",
                                  SchemesContent.SchemeLengthFilters.Select(x => new SchemeLengthFilter(x.Id, x.Description, filters.schemeLength.Contains(x.Id)))));
            filterGroupModels.Add(new FilterGroupModel("pay", "I can offer",
                                  SchemesContent.PayFilters.Select(x => new PayFilter(x.Id, x.Description, filters.pay.Contains(x.Id)))));

            return View(new HomeModel(filteredSchemes, filterGroupModels));
        }

        public IActionResult Details(string schemeUrl)
        {
            if (!SchemeDetailsModels.TryGetValue(schemeUrl, out SchemeDetailsModel? schemeDetailsModel))
                return NotFound();

            return View(schemeDetailsModel);
        }
    }
}
