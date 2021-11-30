using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class HomeModel
    {
        public IEnumerable<Scheme> Schemes { get; set; }

        public IEnumerable<FilterGroupModel> FilterGroupModels { get; set; }

        public HomeModel(IEnumerable<Scheme> schemes, IEnumerable<FilterGroupModel> filterGroupModels)
        {
            Schemes = schemes;
            FilterGroupModels = filterGroupModels;
        }
    }
}
