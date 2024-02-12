using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class ComparisonResultsModel : LayoutModel
    {
        public IEnumerable<Scheme> Schemes { get; }

        public SchemeFilterModel? Filters { get; }

        public ComparisonResultsModel(IEnumerable<Scheme> schemes, SchemeFilterModel? filters, IEnumerable<InterimMenuItem> menuItems)
        {

            Schemes = schemes;

            Filters = filters;

            MenuItems = menuItems;

        }
    }
}
