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

        /// <summary>
        /// Guaranteed in descending Size order
        /// </summary>
        public IEnumerable<Scheme> Schemes { get; }

        /// <summary>
        /// Guaranteed in ascending Order order
        /// </summary>
        public IEnumerable<MotivationsFilter> MotivationsFilters { get; }

        /// <summary>
        /// Guaranteed in ascending Order order
        /// </summary>
        public IEnumerable<PayFilter> PayFilters { get; }

        /// <summary>
        /// Guaranteed in ascending Order order
        /// </summary>
        public IEnumerable<SchemeLengthFilter> SchemeLengthFilters { get; }
    }
}
