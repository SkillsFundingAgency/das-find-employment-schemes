
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;


namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
    public class PreviewContent : Content, IPreviewContent
    {
        public PreviewContent(
            IEnumerable<Page> pages,
            IEnumerable<CaseStudyPage> caseStudyPages,
            IEnumerable<PreviewContentError> caseStudyPagesErrors,
            IEnumerable<Scheme> schemes,
            Filter motivationsFilter,
            Filter payFilter,
            Filter schemeLengthFilter) : base(pages, caseStudyPages, schemes, motivationsFilter, payFilter, schemeLengthFilter)
        {
            CaseStudyPagesErrors = caseStudyPagesErrors;
        }

        public IEnumerable<PreviewContentError> CaseStudyPagesErrors { get; set; }
    }
}
