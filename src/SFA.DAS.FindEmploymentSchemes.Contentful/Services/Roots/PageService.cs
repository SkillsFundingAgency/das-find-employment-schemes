using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using Contentful.Core.Models;
using Contentful.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Contentful.Core.Search;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using ApiPage = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.Page;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    public class PageService : ContentRootService, IPageService
    {
        public PageService(HtmlRenderer htmlRenderer) : base(htmlRenderer)
        {
        }

        public async Task<IEnumerable<Page>> GetAll(IContentfulClient contentfulClient)
        {
            ContentfulCollection<ApiPage> apiData = await GetPagesApi(contentfulClient);
            return await PagesToContent(apiData, contentfulClient.IsPreviewClient);
        }

        private Task<ContentfulCollection<ApiPage>> GetPagesApi(IContentfulClient contentfulClient)
        {
            var builder = QueryBuilder<ApiPage>.New.ContentTypeIs("page").Include(1);
            return contentfulClient.GetEntries(builder);
        }

        private Task<Page[]> PagesToContent(ContentfulCollection<ApiPage> pages, bool includeInvalidContent = false)
        {
            return Task.WhenAll(pages.Where(p => includeInvalidContent
                                                 //todo: needed? either preview, where bypassed or non-preview, where shouldn't happen
                                                 || (p != null && p?.Title != null && p?.Url != null))
                .Select(ToContent));
        }

        private async Task<Page> ToContent(ApiPage apiPage)
        {
            return new Page(
                apiPage.Title!,
                apiPage.Url!,
                (await ToHtmlString(apiPage.Content))!);
        }
    }
}
