using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces
{
    public interface IContent
    {
        public IEnumerable<Page> Pages { get; }
        public IEnumerable<Scheme> Schemes { get; }
    }
}
