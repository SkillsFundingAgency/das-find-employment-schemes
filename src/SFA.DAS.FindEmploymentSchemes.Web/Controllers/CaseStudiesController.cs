using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class CaseStudiesController : Controller
    {
        private readonly ICaseStudyPageService _caseStudyPageService;
        private readonly IContentService _contentService;

        public CaseStudiesController(
            ICaseStudyPageService caseStudyPageService,
            IContentService contentService)
        {
            _caseStudyPageService = caseStudyPageService;
            _contentService = contentService;
        }

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
