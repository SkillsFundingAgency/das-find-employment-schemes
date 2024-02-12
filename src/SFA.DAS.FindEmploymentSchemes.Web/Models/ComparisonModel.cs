using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class ComparisonModel : LayoutModel
    {

        public IEnumerable<Scheme> Schemes { get; }

        public bool NoneSelected { get; }

        public ComparisonModel(IEnumerable<Scheme> schemes, IEnumerable<InterimMenuItem> menuItems, bool noneSelected = false)
        {

            Schemes = schemes;

            NoneSelected = noneSelected;

            MenuItems = menuItems;

        }

    }

}