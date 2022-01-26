
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;


namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class CaseStudiesController : Controller
    {
        private readonly ILogger<CaseStudiesController> _log;

        public CaseStudiesController(ILogger<CaseStudiesController> logger)
        {
            _log = logger;
        }

        // we _could_ add cache control parameters to the page content type
        [HttpGet]
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult CaseStudyPage(string pageUrl)
        {
            pageUrl = pageUrl.ToLowerInvariant();
            CaseStudyPage page = SchemesContent.CaseStudyPages.FirstOrDefault(p => p.Url.ToLowerInvariant() == pageUrl);

            if (pageUrl == "error-check") {
                throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
            }

            if (page == null)
                return NotFound();

            return View(page);
        }
    }
}
