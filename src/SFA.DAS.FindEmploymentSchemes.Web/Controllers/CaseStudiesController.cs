using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class CaseStudiesController : Controller
    {
        private readonly ICaseStudyPageService _caseStudyPageService;
        private readonly IContentService _contentService;
        private readonly ILogger<CaseStudiesController> _log;

        public CaseStudiesController(
            ICaseStudyPageService caseStudyPageService,
            IContentService contentService,
            ILogger<CaseStudiesController> logger)
        {
            _caseStudyPageService = caseStudyPageService;
            _contentService = contentService;
            _log = logger;
        }

        //// we _could_ add cache control parameters to the page content type
        //[HttpGet]
        //[ResponseCache(Duration = 60*60, Location = ResponseCacheLocation.Any, NoStore = false)]
        //public IActionResult CaseStudyPage(string pageUrl)
        //{
        //    pageUrl = pageUrl.ToLowerInvariant();
        //    CaseStudyPage page = SchemesContent.CaseStudyPages.FirstOrDefault(p => p.Url.ToLowerInvariant() == pageUrl);

        //    if (pageUrl == "error-check") {
        //        throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");
        //    }

        //    if (page == null)
        //        return NotFound();

        //    return View(page);
        //}

        [HttpGet]
        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult CaseStudyPage(string pageUrl)
        {
            var (viewName, caseStudyPage) = _caseStudyPageService.CaseStudyPage(pageUrl, _contentService.Content);

            if (caseStudyPage == null)
                return NotFound();

            return View(viewName, caseStudyPage);
        }

        [HttpGet]
        public async Task<IActionResult> CaseStudyPagePreview(string pageUrl)
        {
            var previewContent = await _contentService.UpdatePreview();

            var (viewName, caseStudyPage) = _caseStudyPageService.CaseStudyPage(pageUrl, previewContent);

            if (caseStudyPage == null)
                return NotFound();

            return View(viewName ?? "CaseStudyPage", caseStudyPage);
        }
    }
}
