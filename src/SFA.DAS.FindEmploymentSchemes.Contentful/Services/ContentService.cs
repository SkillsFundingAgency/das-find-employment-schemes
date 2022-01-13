using Contentful.Core;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Contentful.Core.Search;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public interface IContent
    {
        public IEnumerable<Page> Pages { get; set; }
    }

    public class Content : IContent
    {
        public Content(IEnumerable<Page> pages)
        {
            Pages = pages;
        }

        public IEnumerable<Page> Pages { get; set; }
    }

    public class ContentService : IContentService
    {
        private readonly IContentfulClient _contentfulClient;
        private readonly HtmlRenderer _htmlRenderer;

        public ContentService(
            IContentfulClient contentfulClient,
            HtmlRenderer htmlRenderer)
        {
            _contentfulClient = contentfulClient;
            _htmlRenderer = htmlRenderer;
        }

        public async Task<IContent> Get()
        {
            return new Content(await GetPages());
        }

        private async Task<IEnumerable<Page>> GetPages()
        {
            var builder = QueryBuilder<Model.Api.Page>.New.ContentTypeIs("page");

            var apiPages = await _contentfulClient.GetEntries<Model.Api.Page>(builder);

            //todo: ctor to accept apipage??
            return await Task.WhenAll(apiPages.Select(ToContent));
        }

        private async Task<Page> ToContent(Model.Api.Page apiPage)
        {
            //todo: can any of these come through as null?
            return new Page(
                apiPage.Title!,
                apiPage.Url!,
                (await ToHtmlString(apiPage.Content))!);
        }

        private async Task<HtmlString?> ToHtmlString(Document? document)
        {
            //todo: code gen needed to check null, check gen code to remember why
            if (document == null)
                return null;

            string unescapedHtml = await _htmlRenderer.ToHtml(document);

            // replace left/right quotation marks with regular old quotation marks
            string html = unescapedHtml.Replace('“', '"').Replace('”', '"');

            // sometimes contentful uses a \r and sometimes a \r\n - nice!
            // we could strip these out instead
            html = html.Replace("\r\n", "\r");
            html = html.Replace("\r", "\r\n");

            return new HtmlString(html);
        }
    }
}
