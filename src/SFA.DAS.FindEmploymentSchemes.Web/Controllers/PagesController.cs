using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _log;
        private readonly IContentService _contentService;

        public PagesController(
            ILogger<PagesController> logger,
            IContentService contentService)
        {
            _log = logger;
            _contentService = contentService;
        }

        // we _could_ add cache control parameters to the page content type
        [ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult Page(string pageUrl)
        {
            pageUrl = pageUrl.ToLowerInvariant();
            var page = _contentService.Content.Pages.FirstOrDefault(p => p.Url.ToLowerInvariant() == pageUrl);

            switch (pageUrl)
            {
                case "cookies":
                    return (page == null ? NotFound() : (IActionResult)View("Cookies", page));

                case "error-check":
                    throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
            }

            if (page == null)
                return NotFound();

            return View(page);
        }
    }
}
