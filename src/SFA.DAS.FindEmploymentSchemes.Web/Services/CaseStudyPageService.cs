using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class CaseStudyPageService : ICaseStudyPageService
    {
        private readonly IContentService _contentService;
        private IReadOnlyDictionary<string, CaseStudyPageModel> _caseStudyPageModels;

#pragma warning disable CS8618
        public CaseStudyPageService(IContentService contentService)
        {
            _contentService = contentService;
            contentService.ContentUpdated += OnContentUpdated;

            BuildModels();
        }
#pragma warning restore CS8618

        private void BuildModels()
        {
            _caseStudyPageModels = BuildCaseStudyPageModelsDictionary();
        }

        private void OnContentUpdated(object? sender, EventArgs args)
        {
            BuildModels();
        }

        private ReadOnlyDictionary<string, CaseStudyPageModel> BuildCaseStudyPageModelsDictionary()
        {
            var caseStudyPageModels = new Dictionary<string, CaseStudyPageModel>();

            foreach (CaseStudyPage caseStudyPage in _contentService.Content.CaseStudyPages)
            {
                caseStudyPageModels.Add(caseStudyPage.Url.ToLowerInvariant(), new CaseStudyPageModel(caseStudyPage));
            }

            return new ReadOnlyDictionary<string, CaseStudyPageModel>(caseStudyPageModels);
        }

        public CaseStudyPageModel? GetCaseStudyPageModel(string pageUrl)
        {
            pageUrl = pageUrl.ToLowerInvariant();

            if (pageUrl == "error-check")
                throw new NotImplementedException("DEADBEEF-DEAD-BEEF-DEAD-BAAAAAAAAAAD");

            _caseStudyPageModels.TryGetValue(pageUrl, out CaseStudyPageModel? caseStudyPageModel);
            return caseStudyPageModel;
        }

        public async Task<CaseStudyPageModel?> GetCaseStudyPageModelPreview(string pageUrl)
        {
            IContent previewContent = await _contentService.UpdatePreview();

            pageUrl = pageUrl.ToLowerInvariant();

            var caseStudyPage = previewContent.CaseStudyPages.FirstOrDefault(p => p.Url.ToLowerInvariant() == pageUrl);
            if (caseStudyPage == null)
                return null;

            return new CaseStudyPageModel(caseStudyPage)
            {
                Preview = new PreviewModel(GetErrors(caseStudyPage))
            };
        }

        private IEnumerable<HtmlString> GetErrors(CaseStudyPage caseStudyPage)
        {
            var errors = new List<HtmlString>();

            if (string.IsNullOrWhiteSpace(caseStudyPage.Title))
            {
                errors.Add(new HtmlString("Title must not be blank"));
            }

            // note: schemes without urls will have already been filtered out
            bool hasValidScheme = caseStudyPage.Scheme?.Name != null;
            if (!hasValidScheme)
            {
                errors.Add(new HtmlString("Scheme must be selected and have been given an URL and name before publishing"));
            }
            if (caseStudyPage.Content == null)
            {
                errors.Add(new HtmlString("Content must not be blank"));
            }

            return errors;
        }
    }
}
