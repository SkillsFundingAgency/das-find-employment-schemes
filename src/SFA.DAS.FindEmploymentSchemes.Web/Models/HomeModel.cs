using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class HomeModel
    {
        public IHtmlContent Preamble { get; }
        public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<FilterGroupModel> FilterGroupModels { get; }
        public bool EnsureSchemesAreVisible { get; }

        public HomeModel(
            IHtmlContent preamble,
            IEnumerable<Scheme> schemes,
            IEnumerable<FilterGroupModel> filterGroupModels,
            bool ensureSchemesAreVisible = false)
        {
            Preamble = preamble;
            Schemes = schemes;
            FilterGroupModels = filterGroupModels;
            EnsureSchemesAreVisible = ensureSchemesAreVisible;
        }
    }
}