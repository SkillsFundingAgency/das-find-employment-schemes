using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces
{
    public interface IContent
    {
        IEnumerable<Page> Pages { get; }
        IEnumerable<CaseStudyPage> CaseStudyPages { get; }

        /// <summary>
        /// Guaranteed in descending Size order
        /// </summary>
        IEnumerable<Scheme> Schemes { get; }

        Filter MotivationsFilter { get; }
        Filter SchemeLengthFilter { get; }
        Filter PayFilter { get; }
    }
}
