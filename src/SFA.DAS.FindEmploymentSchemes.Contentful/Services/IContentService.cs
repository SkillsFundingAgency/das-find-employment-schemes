
using System;
using System.Threading.Tasks;
using Contentful.Core;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;


namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public interface IContentService
    {
        event EventHandler<EventArgs>? ContentUpdated;
        event EventHandler<EventArgs>? PreviewContentUpdated;

        IContent Content { get; }
        IPreviewContent? PreviewContent { get; }

        Task<IContent> Update();
        Task<IContent> UpdatePreview();
        Task<IPreviewContent> UpdatePreviewSchemeContent(string url);
        Task<IPreviewContent> UpdatePreviewCaseStudyPageContent(string url);
        IPreviewContent UpdatePreviewPageContent(string url);
    }
}
