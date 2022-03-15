using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class CaseStudiesController : Controller
    {
        private readonly ICaseStudyPageService _caseStudyPageService;

        public CaseStudiesController(ICaseStudyPageService caseStudyPageService)
        {
            _caseStudyPageService = caseStudyPageService;
        }

        [HttpGet]
        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult CaseStudyPage(string pageUrl)
        {
            var model = _caseStudyPageService.GetCaseStudyPageModel(pageUrl);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CaseStudyPagePreview(string pageUrl)
        {
            var model = await _caseStudyPageService.GetCaseStudyPageModelPreview(pageUrl);
            if (model == null)
                return NotFound();

            return View("CaseStudyPage", model);
        }
    }
}
