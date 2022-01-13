using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    public class Content : IContent
    {
        public Content(
            IEnumerable<Page> pages,
            IEnumerable<Scheme> schemes)
        {
            Pages = pages;
            Schemes = schemes;
        }

        public IEnumerable<Page> Pages { get; }
        /// <summary>
        /// Guaranteed in descending Size order
        /// </summary>
        public IEnumerable<Scheme> Schemes { get; }
    }
}
