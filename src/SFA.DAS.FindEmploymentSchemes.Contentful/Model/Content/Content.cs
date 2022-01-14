using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    public class Content : IContent
    {
        public Content(
            IEnumerable<Page> pages,
            IEnumerable<Scheme> schemes,
            IEnumerable<MotivationsFilter> motivationsFilters,
            IEnumerable<PayFilter> payFilters,
            IEnumerable<SchemeLengthFilter> schemeLengthFilters)
        {
            Pages = pages;
            Schemes = schemes;
            MotivationsFilters = motivationsFilters;
            PayFilters = payFilters;
            SchemeLengthFilters = schemeLengthFilters;
        }

        public IEnumerable<Page> Pages { get; }
        public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<MotivationsFilter> MotivationsFilters { get; }
        public IEnumerable<PayFilter> PayFilters { get; }
        public IEnumerable<SchemeLengthFilter> SchemeLengthFilters { get; }
    }
}
