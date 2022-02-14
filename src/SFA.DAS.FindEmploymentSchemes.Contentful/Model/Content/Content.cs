using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    public class Content : IContent
    {
        public Content(
            IEnumerable<Page> pages,
            IEnumerable<CaseStudyPage> caseStudyPages,
            IEnumerable<Scheme> schemes,
            Filter motivationsFilter,
            Filter payFilter,
            Filter schemeLengthFilter)
        {
            Pages = pages;
            CaseStudyPages = caseStudyPages;
            Schemes = schemes;
            MotivationsFilter = motivationsFilter;
            PayFilter = payFilter;
            SchemeLengthFilter = schemeLengthFilter;
        }

        public IEnumerable<Page> Pages { get; }
        public IEnumerable<CaseStudyPage> CaseStudyPages { get; }
        public IEnumerable<Scheme> Schemes { get; }
        public Filter MotivationsFilter { get; }
        public Filter PayFilter { get; }
        public Filter SchemeLengthFilter { get; }
    }
}
