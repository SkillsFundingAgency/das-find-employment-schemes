using Contentful.Core;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces
{
    public interface IContentfulClientFactory
    {
        public IContentfulClient? ContentfulClient { get; }
        public IContentfulClient? PreviewContentfulClient { get; }
    }
}
