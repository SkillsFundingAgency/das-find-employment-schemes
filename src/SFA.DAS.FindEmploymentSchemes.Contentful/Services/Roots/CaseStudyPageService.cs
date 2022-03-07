using Contentful.Core;
using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Contentful.Core.Search;
using System.Linq;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using ApiCaseStudyPage = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.CaseStudyPage;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    public class CaseStudyPageService : ContentRootService, ICaseStudyPageService
    {
        private readonly ILogger<CaseStudyPageService> _logger;

        public CaseStudyPageService(
            HtmlRenderer htmlRenderer,
            ILogger<CaseStudyPageService> logger) : base(htmlRenderer)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<CaseStudyPage>> GetAll(
            IContentfulClient contentfulClient, IEnumerable<Scheme> schemes)
        {
            ContentfulCollection<ApiCaseStudyPage> caseStudyPages = await GetCaseStudyPagesFromApi(contentfulClient);

            return await Task.WhenAll(FilterValidUrl(caseStudyPages, _logger).Select(csp => ToContent(csp, schemes)));
        }

        private Task<ContentfulCollection<ApiCaseStudyPage>> GetCaseStudyPagesFromApi(IContentfulClient contentfulClient)
        {
            var builder = QueryBuilder<ApiCaseStudyPage>.New.ContentTypeIs("caseStudyPage").Include(1);
            return contentfulClient.GetEntries(builder);
        }

        private async Task<CaseStudyPage> ToContent(ApiCaseStudyPage apiCaseStudyPage, IEnumerable<Scheme> schemes)
        {
            Scheme? scheme = null;
            string? schemeName = apiCaseStudyPage.Scheme?.Name;
            if (schemeName != null)
            {
                scheme = schemes.FirstOrDefault(x => x.Name == schemeName);
            }

            return new CaseStudyPage(
                apiCaseStudyPage.Title!,
                apiCaseStudyPage.Url!,
                scheme,
                (await ToHtmlString(apiCaseStudyPage.Content))!);
        }
    }
}
