using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class PageModel : LayoutModel
    {

        public Page Page { get; }

        public string? ViewName { get; }

        public PageModel(Page page, IEnumerable<InterimMenuItem> menuItems, BetaBanner? banner, InterimFooterLinks? links, string? viewName = null)
        {

            Page = page;

            ViewName = viewName;

            MenuItems = menuItems;

            BetaBanner = banner;

            InterimFooterLinks = links;

        }

    }

}
