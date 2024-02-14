using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class ComparisonModel : LayoutModel
    {

        public SchemeComparison? SchemeComparison { get; }

        public IEnumerable<Scheme> Schemes { get; }

        public bool NoneSelected { get; }

        public ComparisonModel(
            
            SchemeComparison? comparison, 
            
            IEnumerable<Scheme> schemes, 
            
            IEnumerable<InterimMenuItem> menuItems, 

            BetaBanner? banner,
            
            bool noneSelected = false
            
        )
        {

            SchemeComparison = comparison;

            Schemes = schemes;

            NoneSelected = noneSelected;

            MenuItems = menuItems;

            BetaBanner = banner;

        }

    }

}