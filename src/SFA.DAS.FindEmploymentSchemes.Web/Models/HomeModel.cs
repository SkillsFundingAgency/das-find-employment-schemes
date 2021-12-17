using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class HomeModel
    {
        public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<FilterGroupModel> FilterGroupModels { get; }
        public bool EnsureSchemesAreVisible { get; }

        public HomeModel(IEnumerable<Scheme> schemes, IEnumerable<FilterGroupModel> filterGroupModels, bool ensureSchemesAreVisible = false)
        {
            Schemes = schemes;
            FilterGroupModels = filterGroupModels;
            EnsureSchemesAreVisible = ensureSchemesAreVisible;
        }
    }
}