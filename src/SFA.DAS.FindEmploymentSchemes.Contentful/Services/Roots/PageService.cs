using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using Contentful.Core.Models;
using Contentful.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Contentful.Core.Search;
using System.Linq;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using ApiPage = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.Page;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    public class PageService : ContentRootService, IPageService
    {
        private readonly ILogger<PageService> _logger;

        public PageService(
            HtmlRenderer htmlRenderer,
            ILogger<PageService> logger) : base(htmlRenderer)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Page>> GetAll(IContentfulClient contentfulClient)
        {
            var builder = QueryBuilder<ApiPage>.New.ContentTypeIs("page").Include(1);
            var pages = await contentfulClient.GetEntries(builder);
            LogErrors(pages);

            return await Task.WhenAll(FilterValidUrl(pages, _logger).Select(ToContent));
        }

        //todo: ctor on Page?
        private async Task<Page> ToContent(ApiPage apiPage)
        {
            return new Page(
                apiPage.Title!,
                apiPage.Url!,
                (await ToHtmlString(apiPage.Content))!);
        }
    }
}
