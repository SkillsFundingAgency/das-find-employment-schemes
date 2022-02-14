using Contentful.Core;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public interface IContentfulClientFactory
    {
        public IContentfulClient? ContentfulClient { get; }
        public IContentfulClient? PreviewContentfulClient { get; }
    }
}
