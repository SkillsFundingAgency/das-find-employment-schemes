using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class ComparisonModel : LayoutModel
    {
        public IEnumerable<Scheme> Schemes { get; }

        public ComparisonModel(IEnumerable<Scheme> schemes)
        {
            Schemes = schemes;
        }
    }
}