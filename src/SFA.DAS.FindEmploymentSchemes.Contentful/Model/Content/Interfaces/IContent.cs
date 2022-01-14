using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces
{
    public interface IContent
    {
        IEnumerable<Page> Pages { get; }

        /// <summary>
        /// Guaranteed in descending Size order
        /// </summary>
        IEnumerable<Scheme> Schemes { get; }

        /// <summary>
        /// Guaranteed in ascending Order order
        /// </summary>
        IEnumerable<MotivationsFilter> MotivationsFilters { get; }

        /// <summary>
        /// Guaranteed in ascending Order order
        /// </summary>
        IEnumerable<PayFilter> PayFilters { get; }

        /// <summary>
        /// Guaranteed in ascending Order order
        /// </summary>
        IEnumerable<SchemeLengthFilter> SchemeLengthFilters { get; }
    }
}
