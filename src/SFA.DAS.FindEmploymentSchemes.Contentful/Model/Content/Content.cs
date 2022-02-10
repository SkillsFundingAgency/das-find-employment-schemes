using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    public class Content : IContent
    {
        public Content(
            IEnumerable<Page> pages,
            IEnumerable<Scheme> schemes,
            IEnumerable<SubScheme> subSchemes,
            Filter motivationsFilter,
            Filter payFilter,
            Filter schemeLengthFilter)
        {
            Pages = pages;
            Schemes = schemes;
            SubSchemes = subSchemes;
            MotivationsFilter = motivationsFilter;
            PayFilter = payFilter;
            SchemeLengthFilter = schemeLengthFilter;
        }

        public IEnumerable<Page> Pages { get; }
        public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<SubScheme> SubSchemes { get; }
        public Filter MotivationsFilter { get; }
        public Filter PayFilter { get; }
        public Filter SchemeLengthFilter { get; }
    }
}
