using System;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;

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

        InterimPage? GetInterimPageByURL(string url);

        InterimPage? GetPreviewInterimPageByURL(string url);

    }
}
