using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class HomeModel
    {
        public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<FilterGroupModel> FilterGroupModels { get; }
        public bool ShowSchemesOnMobileNoJavascript { get; }

        public HomeModel(IEnumerable<Scheme> schemes, IEnumerable<FilterGroupModel> filterGroupModels, bool showSchemesOnMobileNoJavascript = false)
        {
            Schemes = schemes;
            FilterGroupModels = filterGroupModels;
            ShowSchemesOnMobileNoJavascript = showSchemesOnMobileNoJavascript;
        }
    }
}