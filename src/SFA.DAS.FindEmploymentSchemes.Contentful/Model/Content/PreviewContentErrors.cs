
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;


namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
    public class PreviewContentErrors : IPreviewContent
    {
        public PreviewContentErrors(
            IEnumerable<Error> errors
            //IEnumerable<Page> pages,
            //IEnumerable<CaseStudyPage> caseStudyPages,
            //IEnumerable<Scheme> schemes,
            //Filter motivationsFilter,
            //Filter payFilter,
            //Filter schemeLengthFilter
            )
        {
            //Pages = pages;
            //CaseStudyPages = caseStudyPages;
            //Schemes = schemes;
            Errors = errors;
            //MotivationsFilter = motivationsFilter;
            //PayFilter = payFilter;
            //SchemeLengthFilter = schemeLengthFilter;
        }

        //public IEnumerable<Page> Pages { get; }
        //public IEnumerable<CaseStudyPage> CaseStudyPages { get; }
        //public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<Error> Errors { get; }
        //public Filter MotivationsFilter { get; }
        //public Filter PayFilter { get; }
        //public Filter SchemeLengthFilter { get; }
    }
}
