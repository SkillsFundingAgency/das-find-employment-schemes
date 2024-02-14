using Contentful.Core;
using Contentful.Core.Models;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions;
using SFA.DAS.FindEmploymentSchemes.Contentful.GdsHtmlRenderers;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using System;
using System.Linq;
using System.Threading.Tasks;
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public class ContentService : IContentService
    {
        private readonly IContentfulClient? _contentfulClient;
        private readonly IContentfulClient? _previewContentfulClient;
        private readonly ISchemeService _schemeService;
        private readonly IPageService _pageService;
        private readonly ICaseStudyPageService _caseStudyPageService;
        private readonly ISchemeFilterService _schemeFilterService;
        private readonly IContactService _contactService;
        private readonly IInterimService _interimService;
        private readonly ILogger<ContentService> _logger;

        public event EventHandler<EventArgs>? ContentUpdated;
        public event EventHandler<EventArgs>? PreviewContentUpdated;

        public ContentService(
            IContentfulClientFactory contentfulClientFactory,
            ISchemeService schemeService,
            IPageService pageService,
            ICaseStudyPageService caseStudyPageService,
            ISchemeFilterService schemeFilterService,
            IContactService contactService,
            IInterimService interimService,
            ILogger<ContentService> logger)
        {
            _contentfulClient = contentfulClientFactory.ContentfulClient;
            _previewContentfulClient = contentfulClientFactory.PreviewContentfulClient;
            _schemeService = schemeService;
            _pageService = pageService;
            _caseStudyPageService = caseStudyPageService;
            _schemeFilterService = schemeFilterService;
            _contactService = contactService;
            _interimService = interimService;
            _logger = logger;
        }

        public static readonly IContent GeneratedContent = new GeneratedContent();

        public IContent Content { get; private set; } = GeneratedContent;
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

            IContent previewContent = await Update(_previewContentfulClient);
            PreviewContent = previewContent;

            _logger.LogInformation("Publishing PreviewContentUpdated event");
            PreviewContentUpdated?.Invoke(this, EventArgs.Empty);
            return previewContent;
        }

        private async Task<IContent> Update(IContentfulClient contentfulClient)
        {

            var schemes = await _schemeService.GetAll(contentfulClient);

            var interimLandingPage = await _interimService.GetLandingPage(contentfulClient);

            var interimMenuItems = await _interimService.GetMenuItems(contentfulClient);

            var betaBanner = await _interimService.GetBetaBanner(contentfulClient);

            var interimPages = await _interimService.GetInterimPages(contentfulClient);

            var schemeComparison = await _schemeService.GetSchemeComparison(contentfulClient); 

            return new Model.Content.Content(

                await _pageService.GetAll(contentfulClient),

                await _caseStudyPageService.GetAll(contentfulClient, schemes),

                schemes,

                await _schemeFilterService.GetSchemeFilters(contentfulClient),

                await _contactService.GetContactPage(contentfulClient),

                interimLandingPage,

                interimMenuItems,

                interimPages,

                schemeComparison,

                betaBanner

            );

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public InterimPage? GetInterimPageByURL(string url)
        {

            try
            {

                return this.Content.InterimPages.FirstOrDefault(a => a.InterimPageURL == url);

            }
            catch(Exception _exception)
            {

                _logger.LogError(_exception, "Unable to get interim page by url: {URL}", url);

                return null;

            }

        }

    }

}
