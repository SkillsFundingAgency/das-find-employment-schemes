using Contentful.Core;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Exceptions;
using IContent = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces.IContent;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public class ContentServiceRF
    {
        private readonly IContentfulClient? _contentfulClient;
        private readonly IContentfulClient? _previewContentfulClient;
        private readonly ILogger<ContentService> _logger;

        public event EventHandler<EventArgs>? ContentUpdated;
        public event EventHandler<EventArgs>? PreviewContentUpdated;

        public ContentServiceRF(
            IContentfulClientFactory contentfulClientFactory,
            ILogger<ContentService> logger)
        {
            _contentfulClient = contentfulClientFactory.ContentfulClient;
            _previewContentfulClient = contentfulClientFactory.PreviewContentfulClient;
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

        private Task<IContent> Update(IContentfulClient contentfulClient)
        {
            throw new NotImplementedException();
        }
    }
}
