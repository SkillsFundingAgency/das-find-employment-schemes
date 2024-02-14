using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{

    [ExcludeFromCodeCoverage]
    public class ComparisonResultsModel : LayoutModel
    {

        public SchemeComparison? SchemeComparison { get; set; }

        public IEnumerable<Scheme> Schemes { get; }

        public SchemeFilterModel? Filters { get; }

        public ComparisonResultsModel(
            
            SchemeComparison? schemeComparison, 
            
            IEnumerable<Scheme> schemes, 
            
            SchemeFilterModel? filters, 
            
            IEnumerable<InterimMenuItem> menuItems,

            BetaBanner? banner
            
        )
        {

            SchemeComparison = schemeComparison;

            Schemes = schemes;

            Filters = filters;

            MenuItems = menuItems;

            BetaBanner = banner;

        }

    }

}
