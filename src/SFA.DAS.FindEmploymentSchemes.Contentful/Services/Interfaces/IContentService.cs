using System;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces
{
    public interface IContentService
    {
        event EventHandler<EventArgs>? ContentUpdated;
        event EventHandler<EventArgs>? PreviewContentUpdated;

        IContent Content { get; }
        IContent? PreviewContent { get; }

        Task<IContent> Update();
        Task<IContent> UpdatePreview();
    }
}
