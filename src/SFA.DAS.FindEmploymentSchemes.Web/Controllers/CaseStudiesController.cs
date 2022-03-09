﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Controllers
{
    public class CaseStudiesController : Controller
    {
        private readonly ICaseStudyPageService _caseStudyPageService;
        private readonly ICaseStudyPageModelService _caseStudyPageModelService;
        private readonly IContentService _contentService;

        public CaseStudiesController(
            ICaseStudyPageService caseStudyPageService,
            ICaseStudyPageModelService caseStudyPageModelService,
            IContentService contentService)
        {
            _caseStudyPageService = caseStudyPageService;
            _caseStudyPageModelService = caseStudyPageModelService;
            _contentService = contentService;
        }

        [HttpGet]
        [ResponseCache(Duration = 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult CaseStudyPage(string pageUrl)
        {
            var (viewName, caseStudyPage) = _caseStudyPageService.CaseStudyPage(pageUrl, _contentService.Content);

            if (caseStudyPage == null)
                return NotFound();

            return View(viewName, (caseStudyPage, PreviewModel.NotPreviewModel));
        }

        [HttpGet]
        public async Task<IActionResult> CaseStudyPagePreview(string pageUrl)
        {
            IContent previewContent = await _contentService.UpdatePreview();

            var (viewName, caseStudyPage) = _caseStudyPageService.CaseStudyPage(pageUrl, previewContent);

            if (caseStudyPage == null)
                return NotFound();

            var previewModel = new PreviewModel(_caseStudyPageModelService.GetErrors(caseStudyPage));

            return View(viewName ?? "CaseStudyPage", (caseStudyPage, previewModel));
        }
    }
}
