﻿
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
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;
using ContentScheme = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Scheme;
using ContentPage = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Page;
using ContentCaseStudyPage = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.CaseStudyPage;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;


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
        //public event EventHandler<EventArgs>? PreviewContentUpdated;

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
        public IPreviewContent? PreviewContent { get; private set; }

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





        // TODO: Replace and delete me!
        public async Task<IPreviewContent> UpdatePreview()
        {
            _logger.LogInformation("Updating preview content");

            if (_previewContentfulClient == null)
                throw new ContentServiceException("Can't update preview content without a preview ContentfulClient.");

            IPreviewContent previewContent = await UpdatePreviewCaseStudyPageContent(""); // _previewContentfulClient);
            //IContent? previewContent = null;

            //if (!previewContent.Errors.Any())
            //{
            //    previewContent = await Update(_previewContentfulClient);
            PreviewContent = previewContent;

            //    _logger.LogInformation("Publishing PreviewContentUpdated event");
            //    PreviewContentUpdated?.Invoke(this, EventArgs.Empty);
            //}

            return previewContent;
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

        public async Task<IPreviewContent> UpdatePreviewCaseStudyPageContent(string url)
        {
            _logger.LogInformation($"Updating content for Case Study Page with url \"{url}\"");
            if (_contentfulClient == null)
                throw new ContentServiceException("Can't preview case study page content without a ContentfulClient.");
            if (_previewContentfulClient == null)
                throw new ContentServiceException("Can't preview content without a PreviewContentfulClient.");

            IEnumerable<Scheme> schemes = await GetSchemes(_contentfulClient);

            IPreviewContent previewContent = new Model.Content.PreviewContent(
                Enumerable.Empty<Model.Content.Page>(),
                Enumerable.Empty<CaseStudyPage>(),
                Enumerable.Empty<PreviewContentError> (),
                schemes,
                new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>()),
                new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>()),
                new Model.Content.Filter(MotivationName, MotivationDescription, Enumerable.Empty<FilterAspect>())
            );
            GetPreviewCaseStudyPages(ref previewContent, _previewContentfulClient, schemes, url);
            return previewContent;
        }

        private IEnumerable<PreviewContentError> GetPreviewErrors<T>(ContentfulCollection<T> contentfulCollection)
        {
            return contentfulCollection.Errors.Select(e => new PreviewContentError(e.Details.Id, e.Details.Type, e.Details.LinkType));
        }

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

        private async Task<IEnumerable<Model.Content.Page>> GetPages(IContentfulClient contentfulClient)
        {
            var builder = QueryBuilder<Model.Api.Page>.New.ContentTypeIs("page");

            var apiPages = await contentfulClient.GetEntries(builder);
            LogErrors(apiPages);

            return await Task.WhenAll(apiPages.Select(ToContent));
        }







        private async Task<IEnumerable<ContentCaseStudyPage>> GetCaseStudyPages(IContentfulClient contentfulClient, IEnumerable<Model.Content.Scheme> schemes)
        {
            //var builder = QueryBuilder<Model.Api.CaseStudyPage>.New.ContentTypeIs("caseStudyPage").Include(1);

            //ContentfulCollection<Model.Api.CaseStudyPage> apiCaseStudyPages = await contentfulClient.GetEntries(builder);
            //LogErrors(apiCaseStudyPages);
            //return await CaseStudyPagesToContent(apiCaseStudyPages, schemes);

            ContentfulCollection<Model.Api.CaseStudyPage> apiData = await GetCaseStudyPagesApi(contentfulClient);
            return await CaseStudyPagesToContent(apiData, schemes, false);
        }

        private void GetPreviewCaseStudyPages(ref IPreviewContent previewContent, IContentfulClient contentfulClient, IEnumerable<Model.Content.Scheme> schemes, string url = "")
        {
            ContentfulCollection<Model.Api.CaseStudyPage> apiData = GetCaseStudyPagesApi(contentfulClient, url).Result;
            previewContent.CaseStudyPages = CaseStudyPagesToContent(apiData, schemes, true).Result;
            previewContent.CaseStudyPagesErrors = GetPreviewErrors(apiData);
            //return previewContent;
        }

        private async Task<ContentfulCollection<Model.Api.CaseStudyPage>> GetCaseStudyPagesApi(IContentfulClient contentfulClient, string url = "") //, IEnumerable<Model.Content.Scheme> schemes)
        {
            var builder = QueryBuilder<Model.Api.CaseStudyPage>.New.ContentTypeIs("caseStudyPage").Include(1);
            if (!string.IsNullOrWhiteSpace(url))
                builder = builder.FieldEquals(x => x.Url, url);
            ContentfulCollection<Model.Api.CaseStudyPage> apiCaseStudyPages = await contentfulClient.GetEntries(builder);
            return apiCaseStudyPages;
        }

        private Task<ContentCaseStudyPage[]> CaseStudyPagesToContent(ContentfulCollection<Model.Api.CaseStudyPage> caseStudyPages, IEnumerable<Model.Content.Scheme> schemes, bool includeInvalidContent = false)
        {
            return Task.WhenAll(caseStudyPages.Where(csp => includeInvalidContent || (csp != null && csp?.Scheme != null && csp?.Title != null && csp?.Url != null))
                                              .Select(csp => ToContent(csp, schemes)));
        }








        private async Task<IEnumerable<Model.Content.Scheme>> GetSchemes(IContentfulClient contentfulClient)
        {
            var builder = QueryBuilder<Model.Api.Scheme>.New.ContentTypeIs("scheme").Include(1);

            var schemes = await contentfulClient.GetEntries(builder);
            LogErrors(schemes);

            return await Task.WhenAll(schemes.OrderByDescending(s => s.Size).Select(ToContent));
        }

        private async Task<IEnumerable<Model.Content.FilterAspect>> GetFilterAspects(
            IContentfulClient contentfulClient,
            string contentfulTypeName,
            string filterPrefix)
        {
            var builder = QueryBuilder<Model.Api.Filter>.New.ContentTypeIs(contentfulTypeName);

            var filterAspects = await contentfulClient.GetEntries(builder);
            LogErrors(filterAspects);

            return filterAspects.OrderBy(f => f.Order).Select(f => ToContent(f, filterPrefix));
        }

        private async Task<Model.Content.Page> ToContent(Model.Api.Page apiPage)
        {
            return new Model.Content.Page(
                apiPage.Title!,
                apiPage.Url!,
                (await ToHtmlString(apiPage.Content))!);
        }

        private async Task<ContentCaseStudyPage> ToContent(Model.Api.CaseStudyPage apiCaseStudyPage, IEnumerable<Model.Content.Scheme> schemes)
        {
            //ContentScheme scheme = schemes.FirstOrDefault(x => x.Name == apiCaseStudyPage?.Scheme?.Name); // apiCaseStudyPage.Scheme!.Name)
            return new ContentCaseStudyPage(
                apiCaseStudyPage.Title!,
                apiCaseStudyPage.Url!,
                schemes.FirstOrDefault(x => x.Name == apiCaseStudyPage?.Scheme?.Name),
                (await ToHtmlString(apiCaseStudyPage.Content))!);
        }

        private async Task<Model.Content.Scheme> ToContent(Model.Api.Scheme apiScheme)
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

        private Model.Content.FilterAspect ToContent(Model.Api.IFilter apiFilter, string filterPrefix)
        {
            return new Model.Content.FilterAspect(ToFilterAspectId(apiFilter, filterPrefix), apiFilter.Description!);
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
