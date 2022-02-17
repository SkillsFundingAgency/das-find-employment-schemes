using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces
{
    public interface IPreviewContent : IContent
    {
        IEnumerable<PreviewContentErrors> Errors { get; }
    }
}
