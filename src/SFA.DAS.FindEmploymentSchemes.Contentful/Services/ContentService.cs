
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
using SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;
using ContentScheme = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Scheme;
using ContentPage = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Page;
using ContentCaseStudyPage = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.CaseStudyPage;
using ContentFilterAspect = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.FilterAspect;
using ApiScheme = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.Scheme;
using ApiPage = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.Page;
using ApiFilter = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.Filter;
//using PreviewContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.PreviewContent;
using ApiCaseStudyPage = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.CaseStudyPage;


namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public class ContentService : IContentService
    {
        private readonly IContentfulClient? _contentfulClient;
        private readonly IContentfulClient? _previewContentfulClient;
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
        public event EventHandler<EventArgs>? PreviewContentUpdated;

        public ContentService(
            IContentfulClientFactory contentfulClientFactory,
            HtmlRenderer htmlRenderer,
            ILogger<ContentService> logger)
        {
            _contentfulClient = contentfulClientFactory.ContentfulClient;
            _previewContentfulClient = contentfulClientFactory.PreviewContentfulClient;
            _htmlRenderer = htmlRenderer;
            _logger = logger;
        }

        public static readonly IContent GeneratedContent = new GeneratedContent();

        public IContent Content { get; private set; } = GeneratedContent;
        //public IPreviewContent? PreviewContent { get; private set; }
        public IContent? PreviewContent { get; private set; }

        public async Task<IContent> Update()
        {
            _logger.LogInformation("Updating content");

            if (_contentfulClient == null)
                throw new ContentServiceException("Can't update content without a ContentfulClient.");

            var content = await Update(_contentfulClient);
            Content = content;

            _logger.LogInformation("Publishing ContentUpdated event");
            ContentUpdated?.Invoke(this, EventArgs.Empty);

            return content;
        }

        public async Task<IContent> UpdatePreview()
        {
            _logger.LogInformation("Updating preview content");

            if (_previewContentfulClient == null)
                throw new ContentServiceException("Can't update preview content without a preview ContentfulClient.");

            //IContent previewContent = await Update(_previewContentfulClient);
            //PreviewContent = previewContent as PreviewContent;
            PreviewContent = await Update(_previewContentfulClient);

            _logger.LogInformation("Publishing PreviewContentUpdated event");
            PreviewContentUpdated?.Invoke(this, EventArgs.Empty);
            return PreviewContent;
        }

        private async Task<IContent> Update(IContentfulClient contentfulClient)
        {
            var schemes = await GetSchemes(contentfulClient);

            return new Model.Content.Content(
                await GetPages(contentfulClient),
                await GetCaseStudyPages(contentfulClient, schemes),
                schemes,
                new Model.Content.Filter(
                    MotivationName,
                    MotivationDescription,
                    await GetFilterAspects(contentfulClient, MotivationsFilterContentfulTypeName, MotivationsFilterPrefix)),
                new Model.Content.Filter(
                    PayName,
                    PayDescription,
                    await GetFilterAspects(contentfulClient, PayFilterContentfulTypeName, PayFilterPrefix)),
                new Model.Content.Filter(
                    SchemeLengthName,
                    SchemeLengthDescription,
                    await GetFilterAspects(contentfulClient, SchemeLengthFilterContentfulTypeName, SchemeLengthFilterPrefix)));
        }

        //public async Task<IPreviewContent> UpdatePreviewCaseStudyPageContent(string url)
        //{
        //    _logger.LogInformation($"Updating content for Case Study Page with url \"{url}\"");
        //    if (_previewContentfulClient == null)
        //        throw new ContentServiceException("Can't preview content without a PreviewContentfulClient.");

        //    IEnumerable<Scheme> schemes = await GetSchemes(_previewContentfulClient);
        //    return GetPreviewCaseStudyPages(_previewContentfulClient, schemes, url);
        //}

        //public IPreviewContent UpdatePreviewPageContent(string url)
        //{
        //    _logger.LogInformation($"Updating content for Page with url \"{url}\"");
        //    if (_previewContentfulClient == null)
        //        throw new ContentServiceException("Can't preview content without a PreviewContentfulClient.");

        //    return GetPreviewPages(_previewContentfulClient, url);
        //}

        //public async Task<IPreviewContent> UpdatePreviewSchemeContent(string url)
        //{
        //    _logger.LogInformation($"Updating content for Scheme with url \"{url}\"");
        //    if (_previewContentfulClient == null)
        //        throw new ContentServiceException("Can't preview content without a PreviewContentfulClient.");

        //    IEnumerable<Scheme> schemes = await GetSchemes(_previewContentfulClient);
        //    IPreviewContent previewContent = GetPreviewSchemes(_previewContentfulClient, url);
        //    IEnumerable<Scheme> previewSchemes = ((IContent)previewContent).Schemes;

        //    if (!string.IsNullOrWhiteSpace(url) && previewSchemes != null && previewSchemes.Count() == 1)
        //        previewContent.Schemes = schemes.Where(s => s.Url != url)
        //                                        .Append(previewSchemes.First())
        //                                        .OrderBy(s => s.Size);
        //    return previewContent;
        //}

        //private IEnumerable<PreviewContentError> GetPreviewErrors<T>(ContentfulCollection<T> contentfulCollection)
        //{
        //    return contentfulCollection.Errors.Select(e => new PreviewContentError(e.Details.Id, e.Details.Type, e.Details.LinkType));
        //}

        private void LogErrors<T>(ContentfulCollection<T> contentfulCollection)
        {
            //todo: log errors
            //todo: show error when previewing

#if log_errors
            if (!contentfulCollection.Errors.Any())
                return;

            //todo: log SystemProperties.Type?
            _logger.LogWarning($"Errors received fetching {nameof(T)}'s.");

            foreach (var errorDetails in contentfulCollection.Errors.Select(e => e.Details))
            {
                _logger.LogWarning($"Id:{errorDetails.Id}, LinkType:{errorDetails.LinkType}, Type:{errorDetails.Type}");
            }
#endif
        }

        private async Task<IEnumerable<ContentPage>> GetPages(IContentfulClient contentfulClient)
        {
            ContentfulCollection<ApiPage> apiData = await GetPagesApi(contentfulClient);
            return await PagesToContent(apiData, contentfulClient.IsPreviewClient);
        }

        private async Task<IEnumerable<ContentCaseStudyPage>> GetCaseStudyPages(IContentfulClient contentfulClient, IEnumerable<ContentScheme> schemes)
        {
            ContentfulCollection<ApiCaseStudyPage> apiData = await GetCaseStudyPagesApi(contentfulClient);
            return await CaseStudyPagesToContent(apiData, schemes, contentfulClient.IsPreviewClient);
        }
        private async Task<IEnumerable<ContentScheme>> GetSchemes(IContentfulClient contentfulClient)
        {
            ContentfulCollection<ApiScheme> apiData = await GetSchemesApi(contentfulClient);
            return await SchemesToContent(apiData, contentfulClient.IsPreviewClient);
        }

        //private IPreviewContent GetPreviewCaseStudyPages(IContentfulClient contentfulClient, IEnumerable<ContentScheme> schemes, string url = "")
        //{
        //    ContentfulCollection<ApiCaseStudyPage> apiData = GetCaseStudyPagesApi(contentfulClient, url).Result;

        //    return new PreviewContent(
        //        Enumerable.Empty<ContentPage>(),
        //        Enumerable.Empty<PreviewContentError>(),
        //        CaseStudyPagesToContent(apiData, schemes, true).Result,
        //        GetPreviewErrors(apiData),
        //        schemes,
        //        Enumerable.Empty<PreviewContentError>(),
        //        new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>()),
        //        new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>()),
        //        new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>())
        //    );
        //}

        //private IPreviewContent GetPreviewPages(IContentfulClient contentfulClient, string url = "")
        //{
        //    ContentfulCollection<ApiPage> apiData = GetPagesApi(contentfulClient, url).Result;

        //    return new PreviewContent(
        //        PagesToContent(apiData, true).Result,
        //        GetPreviewErrors(apiData),
        //        Enumerable.Empty<ContentCaseStudyPage>(),
        //        Enumerable.Empty<PreviewContentError>(),
        //        Enumerable.Empty<ContentScheme>(),
        //        Enumerable.Empty<PreviewContentError>(),
        //        new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>()),
        //        new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>()),
        //        new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>())
        //    );
        //}

        //private IPreviewContent GetPreviewSchemes(IContentfulClient contentfulClient, string url = "")
        //{
        //    ContentfulCollection<ApiScheme> apiData = GetSchemesApi(contentfulClient, url).Result;

        //    return new PreviewContent(
        //        Enumerable.Empty<ContentPage>(),
        //        Enumerable.Empty<PreviewContentError>(),
        //        Enumerable.Empty<ContentCaseStudyPage>(),
        //        Enumerable.Empty<PreviewContentError>(),
        //        SchemesToContent(apiData, true).Result,
        //        GetPreviewErrors(apiData),
        //        new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>()),
        //        new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>()),
        //        new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>())
        //    );
        //}

        private async Task<ContentfulCollection<ApiCaseStudyPage>> GetCaseStudyPagesApi(IContentfulClient contentfulClient, string url = "")
        {
            var builder = QueryBuilder<ApiCaseStudyPage>.New.ContentTypeIs("caseStudyPage").Include(1);
            if (!string.IsNullOrWhiteSpace(url))
                builder = builder.FieldEquals(x => x.Url, url);
            ContentfulCollection<ApiCaseStudyPage> apiCaseStudyPages = await contentfulClient.GetEntries(builder);
            return apiCaseStudyPages;
        }

        private async Task<ContentfulCollection<ApiPage>> GetPagesApi(IContentfulClient contentfulClient) //, string url = "")
        {
            var builder = QueryBuilder<ApiPage>.New.ContentTypeIs("page").Include(1);
            ContentfulCollection<ApiPage> apiPages = await contentfulClient.GetEntries(builder);
            return apiPages;
        }

        private async Task<ContentfulCollection<ApiScheme>> GetSchemesApi(IContentfulClient contentfulClient) //, string url = "")
        {
            var builder = QueryBuilder<ApiScheme>.New.ContentTypeIs("scheme").Include(1);
            ContentfulCollection<ApiScheme> apiSchemes = await contentfulClient.GetEntries(builder);
            return apiSchemes;
        }

        private Task<ContentCaseStudyPage[]> CaseStudyPagesToContent(ContentfulCollection<ApiCaseStudyPage> caseStudyPages, IEnumerable<ContentScheme> schemes, bool includeInvalidContent = false)
        {
            return Task.WhenAll(caseStudyPages.Where(csp => includeInvalidContent || (csp != null && csp?.Scheme != null && csp?.Title != null && csp?.Url != null))
                                              .Select(csp => ToContent(csp, schemes)));
        }

        private Task<ContentPage[]> PagesToContent(ContentfulCollection<ApiPage> pages, bool includeInvalidContent = false)
        {
            return Task.WhenAll(pages.Where(p => includeInvalidContent || (p != null && p?.Title != null && p?.Url != null))
                                     .Select(p => ToContent(p)));
        }

        private Task<ContentScheme[]> SchemesToContent(ContentfulCollection<ApiScheme> schemes, bool includeInvalidContent = false)
        {
            return Task.WhenAll(schemes.Where(s => includeInvalidContent || (s != null && s?.Name != null && s?.ShortDescription != null && s?.ShortBenefits != null && s?.ShortTime != null && s?.Size != null && s?.Url != null))
                                       .OrderByDescending(s => s.Size)
                                       .Select(s => ToContent(s)));
        }

        private async Task<IEnumerable<FilterAspect>> GetFilterAspects(
            IContentfulClient contentfulClient,
            string contentfulTypeName,
            string filterPrefix)
        {
            var builder = QueryBuilder<ApiFilter>.New.ContentTypeIs(contentfulTypeName);

            var filterAspects = await contentfulClient.GetEntries(builder);
            LogErrors(filterAspects);

            return filterAspects.OrderBy(f => f.Order).Select(f => ToContent(f, filterPrefix));
        }

        private async Task<ContentPage> ToContent(ApiPage apiPage)
        {
            return new ContentPage(
                apiPage.Title!,
                apiPage.Url!,
                (await ToHtmlString(apiPage.Content))!);
        }

        private async Task<ContentCaseStudyPage> ToContent(ApiCaseStudyPage apiCaseStudyPage, IEnumerable<ContentScheme> schemes)
        {
            return new ContentCaseStudyPage(
                apiCaseStudyPage.Title!,
                apiCaseStudyPage.Url!,
                schemes.FirstOrDefault(x => x.Name == apiCaseStudyPage?.Scheme?.Name),
                (await ToHtmlString(apiCaseStudyPage.Content))!);
        }

        private async Task<ContentScheme> ToContent(ApiScheme apiScheme)
        {
            IEnumerable<Model.Content.CaseStudy> caseStudies = Enumerable.Empty<Model.Content.CaseStudy>();
            if (apiScheme.CaseStudyReferences != null)
            {
                caseStudies = await Task.WhenAll(apiScheme.CaseStudyReferences.Select(ToContent));
            }

            IEnumerable<Model.Content.SubScheme> subSchemes = Enumerable.Empty<Model.Content.SubScheme>();
            if (apiScheme.SubSchemes != null)
            {
                subSchemes = await Task.WhenAll(apiScheme.SubSchemes.Select(ToContent));
            }

            return new ContentScheme (
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
                caseStudies,
                await ToHtmlString(apiScheme.CaseStudies),
                await ToHtmlString(apiScheme.DetailsPageOverride),
                await ToHtmlString(apiScheme.Description),
                await ToHtmlString(apiScheme.Cost),
                await ToHtmlString(apiScheme.Responsibility),
                await ToHtmlString(apiScheme.Benefits),
                apiScheme.OfferHeader,
                await ToHtmlString(apiScheme.Offer),
                await ToHtmlString(apiScheme.AdditionalFooter),
                subSchemes);
        }

        private async Task<Model.Content.SubScheme> ToContent(Model.Api.SubScheme apiSubScheme)
        {
            return new Model.Content.SubScheme(
                apiSubScheme.Title!,
                await ToHtmlString(apiSubScheme.Summary),
                (await ToHtmlString(apiSubScheme.Content))!);
        }

        private ContentFilterAspect ToContent(Model.Api.IFilter apiFilter, string filterPrefix)
        {
            return new ContentFilterAspect(ToFilterAspectId(apiFilter, filterPrefix), apiFilter.Description!);
        }

        private async Task<Model.Content.CaseStudy> ToContent(Model.Api.CaseStudy apiCaseStudy)
        {
            //todo: check all are mandatory in contentful
            return new Model.Content.CaseStudy(
                apiCaseStudy.Name!,
                apiCaseStudy.DisplayTitle!,
                (await ToHtmlString(apiCaseStudy.Description))!);
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

            string html = await _htmlRenderer.ToHtml(document);

            return ToNormalisedHtmlString(html);
        }

        /// <remarks>
        /// Should be private, but Contentful's .net library is not very test friendly (HtmlRenderer.ToHtml can't be mocked).
        /// We'd have to introduce a level of indirection to test this, were it private.
        /// </remarks>
        public static HtmlString ToNormalisedHtmlString(string html)
        {
            // replace left/right quotation marks with regular old quotation marks
            html = html.Replace('“', '"').Replace('”', '"');

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
            htmlRenderer.AddRenderer(new GdsEmbeddedYoutubeContentRenderer());
            htmlRenderer.AddRenderer(new GdsCtaContentRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsHeadingRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsHorizontalRulerContentRenderer());
            htmlRenderer.AddRenderer(new GdsHyperlinkContentRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsListContentRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsParagraphRenderer(htmlRenderer.Renderers));
            htmlRenderer.AddRenderer(new GdsBlockQuoteRenderer(htmlRenderer.Renderers));

            return htmlRenderer;
        }
    }
}
