
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;


namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
    public class PreviewContent : Content, IPreviewContent
    {
        public PreviewContent(
            IEnumerable<Page>? pages,
            IEnumerable<PreviewContentError> pagesErrors,
            IEnumerable<CaseStudyPage>? caseStudyPages,
            IEnumerable<PreviewContentError> caseStudyPagesErrors,
            IEnumerable<Scheme> schemes,
            IEnumerable<PreviewContentError> schemesErrors,
            Filter motivationsFilter,
            Filter payFilter,
            Filter schemeLengthFilter) : base(pages ?? new Page[] {}, caseStudyPages ?? new CaseStudyPage[] {}, schemes ?? new Scheme[] { }, motivationsFilter, payFilter, schemeLengthFilter)
        {
            SchemesErrors = schemesErrors;
            PagesErrors = pagesErrors;
            CaseStudyPagesErrors = caseStudyPagesErrors;
        }

        public IEnumerable<PreviewContentError> SchemesErrors { get; set; }
        public IEnumerable<PreviewContentError> PagesErrors { get; set; }
        public IEnumerable<PreviewContentError> CaseStudyPagesErrors { get; set; }
        public IEnumerable<PreviewContentError> Errors => SchemesErrors.Union(PagesErrors).Union(CaseStudyPagesErrors);

        IEnumerable<Scheme>? IPreviewContent.Schemes { get; set; }
        IEnumerable<Page>? IPreviewContent.Pages { get; set; }
        IEnumerable<CaseStudyPage>? IPreviewContent.CaseStudyPages { get; set; }
    }
}
