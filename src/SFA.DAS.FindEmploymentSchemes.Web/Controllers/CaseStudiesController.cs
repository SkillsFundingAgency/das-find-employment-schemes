﻿
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;


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
            IPreviewContent previewContent = await _contentService.UpdatePreviewCaseStudyPageContent(pageUrl);

            var (viewName, caseStudyPage) = _caseStudyPageService.CaseStudyPage(pageUrl, previewContent);

            if (caseStudyPage == null)
                return NotFound();

            return View(viewName ?? "CaseStudyPagePreview", new CaseStudyPagePreview(caseStudyPage, previewContent.CaseStudyPagesErrors));
        }
    }
}
