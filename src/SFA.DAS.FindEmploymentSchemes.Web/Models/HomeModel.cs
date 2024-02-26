using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class HomeModel : LayoutModel
    {
        public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<FilterSectionModel> Filters { get; }
        public InterimPreamble? InterimPreamble { get; }
        public InterimBreadcrumbs? InterimPageBreadcrumbs { get; }
        public bool EnsureSchemesAreVisible { get; }
        public string SelectedFilters { get; }

        public HomeModel(
            IEnumerable<Scheme> schemes,
            IEnumerable<FilterSectionModel> filters,
            IEnumerable<InterimMenuItem> menuItems,
            InterimPreamble? interimPreamble,
            InterimBreadcrumbs? interimPageBreadcrumbs,
            BetaBanner? banner,
            InterimFooterLinks? interimFooterLinks,
            bool ensureSchemesAreVisible = false,
            string selectedFilters = "")
        {
            InterimPreamble = interimPreamble;
            InterimPageBreadcrumbs = interimPageBreadcrumbs;
            Schemes = schemes;
            Filters = filters;
            MenuItems = menuItems;
            EnsureSchemesAreVisible = ensureSchemesAreVisible;
            SelectedFilters = selectedFilters;
            BetaBanner = banner;
            InterimFooterLinks = interimFooterLinks;
        }
    }
}