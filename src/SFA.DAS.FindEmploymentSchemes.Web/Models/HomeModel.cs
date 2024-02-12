using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class HomeModel : LayoutModel
    {
        public IHtmlContent? Preamble { get; }
        public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<Filter> Filters { get; }
        public bool EnsureSchemesAreVisible { get; }
        public string SelectedFilters { get; }

        public HomeModel(
            IHtmlContent? preamble,
            IEnumerable<Scheme> schemes,
            IEnumerable<Filter> filters,
            IEnumerable<InterimMenuItem> menuItems,
            bool ensureSchemesAreVisible = false,
            string selectedFilters = "")
        {
            Preamble = preamble;
            Schemes = schemes;
            Filters = filters;
            MenuItems = menuItems;
            EnsureSchemesAreVisible = ensureSchemesAreVisible;
            SelectedFilters = selectedFilters;
        }
    }
}