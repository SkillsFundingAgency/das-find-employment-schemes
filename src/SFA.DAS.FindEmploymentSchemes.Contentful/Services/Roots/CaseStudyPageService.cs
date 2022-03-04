using Contentful.Core;
using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Contentful.Core.Search;
using System.Linq;
using ApiCaseStudyPage = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.CaseStudyPage;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    public class CaseStudyPageService : ContentRootService
    {
        public CaseStudyPageService(HtmlRenderer htmlRenderer) : base(htmlRenderer)
        {
        }

        private async Task<IEnumerable<CaseStudyPage>> GetAll(IContentfulClient contentfulClient, IEnumerable<Scheme> schemes)
        {
            ContentfulCollection<ApiCaseStudyPage> apiData = await GetCaseStudyPagesApi(contentfulClient);
            return await CaseStudyPagesToContent(apiData, schemes, contentfulClient.IsPreviewClient);
        }

        private Task<ContentfulCollection<ApiCaseStudyPage>> GetCaseStudyPagesApi(IContentfulClient contentfulClient)
        {
            var builder = QueryBuilder<ApiCaseStudyPage>.New.ContentTypeIs("caseStudyPage").Include(1);
            return contentfulClient.GetEntries(builder);
        }

        private Task<CaseStudyPage[]> CaseStudyPagesToContent(
            ContentfulCollection<ApiCaseStudyPage> caseStudyPages,
            IEnumerable<Scheme> schemes,
            bool includeInvalidContent = false)
        {
            return Task.WhenAll(caseStudyPages.Where(csp => includeInvalidContent || (csp != null && csp?.Scheme != null && csp?.Title != null && csp?.Url != null))
                .Select(csp => ToContent(csp, schemes)));
        }

        private async Task<CaseStudyPage> ToContent(ApiCaseStudyPage apiCaseStudyPage, IEnumerable<Scheme> schemes)
        {
            return new CaseStudyPage(
                apiCaseStudyPage.Title!,
                apiCaseStudyPage.Url!,
                schemes.FirstOrDefault(x => x.Name == apiCaseStudyPage?.Scheme?.Name),
                (await ToHtmlString(apiCaseStudyPage.Content))!);
        }
    }
}
