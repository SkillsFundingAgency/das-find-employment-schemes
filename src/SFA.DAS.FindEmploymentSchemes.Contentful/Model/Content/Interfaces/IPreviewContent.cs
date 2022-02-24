using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces
{
    public interface IPreviewContent : IContent
    {
        new IEnumerable<Page>? Pages { get; set; }
        IEnumerable<PreviewContentError> PagesErrors { get; set; }
        new IEnumerable<CaseStudyPage>? CaseStudyPages { get; set; }
        IEnumerable<PreviewContentError> CaseStudyPagesErrors { get; set; }
        IEnumerable<PreviewContentError> Errors { get; }
    }
}
