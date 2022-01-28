using Contentful.Core;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contentful.Core.Search;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.GdsHtmlRenderers;
using System;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public class ContentService : IContentService
    {
        private readonly IContentfulClient _contentfulClient;
        private readonly HtmlRenderer _htmlRenderer;
        private readonly ILogger<ContentService> _logger;

        private const string MotivationsFilterPrefix = "motivations";
        private const string PayFilterPrefix = "pay";
        private const string SchemeLengthFilterPrefix = "scheme-length";

        private const string MotivationsFilterContentfulTypeName = "motivationsFilter";
        private const string PayFilterContentfulTypeName = "payFilter";
        private const string SchemeLengthFilterContentfulTypeName = "schemeLengthFilter";

        private const string MotivationName = "motivations";
        private const string MotivationDescription = "I want to";
        private const string SchemeLengthName = "schemeLength";
        private const string SchemeLengthDescription = "Length of scheme?";
        private const string PayName = "pay";
        private const string PayDescription = "I can offer";

        public event EventHandler<EventArgs>? ContentUpdated;

        public ContentService(
            IContentfulClient contentfulClient,
            HtmlRenderer htmlRenderer,
            ILogger<ContentService> logger)
        {
            _contentfulClient = contentfulClient;
            _htmlRenderer = htmlRenderer;
            _logger = logger;
        }

        public static readonly IContent GeneratedContent = new GeneratedContent();

        public IContent Content { get; private set; } = GeneratedContent;

        public async Task<IContent> Update()
        {
            _logger.LogInformation("Updating content");
            Content = new Model.Content.Content(
                await GetPages(),
                await GetSchemes(),
                new Model.Content.Filter(
                    MotivationName,
                    MotivationDescription,
                    await GetFilterAspects(MotivationsFilterContentfulTypeName, MotivationsFilterPrefix)),
                new Model.Content.Filter(
                    PayName,
                    PayDescription,
                    await GetFilterAspects(PayFilterContentfulTypeName, PayFilterPrefix)),
                new Model.Content.Filter(
                    SchemeLengthName,
                    SchemeLengthDescription,
                    await GetFilterAspects(SchemeLengthFilterContentfulTypeName, SchemeLengthFilterPrefix)));

            _logger.LogInformation("Publishing ContentUpdated event");
            ContentUpdated?.Invoke(this, EventArgs.Empty);

            return Content;
        }

        private async Task<IEnumerable<Model.Content.Page>> GetPages()
        {
            var builder = QueryBuilder<Model.Api.Page>.New.ContentTypeIs("page");

            var apiPages = await _contentfulClient.GetEntries(builder);

            return await Task.WhenAll(apiPages.Select(ToContent));
        }

        private async Task<IEnumerable<Model.Content.Scheme>> GetSchemes()
        {
            var builder = QueryBuilder<Model.Api.Scheme>.New.ContentTypeIs("scheme").Include(1);

            var schemes = await _contentfulClient.GetEntries(builder);

            return await Task.WhenAll(schemes.OrderByDescending(s => s.Size).Select(ToContent));
        }

        private async Task<IEnumerable<Model.Content.FilterAspect>> GetFilterAspects(string contentfulTypeName, string filterPrefix)
        {
            var builder = QueryBuilder<Model.Api.Filter>.New.ContentTypeIs(contentfulTypeName);

            var filterAspects = await _contentfulClient.GetEntries(builder);

            return filterAspects.OrderBy(f => f.Order).Select(f => ToContent(f, filterPrefix));
        }

        private async Task<Model.Content.Page> ToContent(Model.Api.Page apiPage)
        {
            return new Model.Content.Page(
                apiPage.Title!,
                apiPage.Url!,
                (await ToHtmlString(apiPage.Content))!);
        }

        private async Task<Model.Content.Scheme> ToContent(Model.Api.Scheme apiScheme)
        {
            return new Model.Content.Scheme(
                apiScheme.Name!,
                (await ToHtmlString(apiScheme.ShortDescription))!,
                (await ToHtmlString(apiScheme.ShortCost))!,
                (await ToHtmlString(apiScheme.ShortBenefits))!,
                (await ToHtmlString(apiScheme.ShortTime))!,
                apiScheme.Url!,
                apiScheme.Size,
                (apiScheme.PayFilterAspects?.Select(f => ToFilterAspectId(f, PayFilterPrefix)) ?? Enumerable.Empty<string>())
                    .Concat(apiScheme.MotivationsFilterAspects?.Select(f => ToFilterAspectId(f, MotivationsFilterPrefix)) ?? Enumerable.Empty<string>())
                    .Concat(apiScheme.SchemeLengthFilterAspects?.Select(f => ToFilterAspectId(f, SchemeLengthFilterPrefix)) ?? Enumerable.Empty<string>()),
                await ToHtmlString(apiScheme.DetailsPageOverride),
                await ToHtmlString(apiScheme.Description),
                await ToHtmlString(apiScheme.Cost),
                await ToHtmlString(apiScheme.Responsibility),
                await ToHtmlString(apiScheme.Benefits),
                await ToHtmlString(apiScheme.CaseStudies),
                apiScheme.OfferHeader,
                await ToHtmlString(apiScheme.Offer),
                await ToHtmlString(apiScheme.AdditionalFooter));
        }

        private Model.Content.FilterAspect ToContent(Model.Api.IFilter apiFilter, string filterPrefix)
        {
            return new Model.Content.FilterAspect(ToFilterAspectId(apiFilter, filterPrefix), apiFilter.Description!);
        }

        private static string ToFilterAspectId(Model.Api.IFilter filter, string filterPrefix)
        {
            return $"{filterPrefix}--{Slugify(filter.Name)}";
        }

        private static string Slugify(string? name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return name.ToLower().Replace(' ', '-');
        }

        private async Task<HtmlString?> ToHtmlString(Document? document)
        {
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

        public static HtmlRenderer CreateHtmlRenderer()
        {
            var htmlRendererOptions = new HtmlRendererOptions
            {
                ListItemOptions =
                {
                    OmitParagraphTagsInsideListItems = true
                }
            };
            var htmlRenderer = new HtmlRenderer(htmlRendererOptions);
            htmlRenderer.AddRenderer(new GdsCtaContentRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsHeadingRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsHorizontalRulerContentRenderer());
            htmlRenderer.AddRenderer(new GdsHyperlinkContentRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsListContentRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsParagraphRenderer(htmlRenderer.Renderers));

            return htmlRenderer;
        }
    }
}
