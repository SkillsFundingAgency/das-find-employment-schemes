
using System.Collections.Generic;


namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class HomeModel
    {
        public bool ShowFilters { get; set; }
        public IEnumerable<Scheme> Schemes { get; set; }

        public IEnumerable<FilterGroupModel> FilterGroupModels { get; set; }

        public HomeModel(IEnumerable<Scheme> schemes, IEnumerable<FilterGroupModel> filterGroupModels, bool showFilters = true)
        {
            ShowFilters = showFilters;
            Schemes = schemes;
            FilterGroupModels = filterGroupModels;
        }
    }
}
