using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{

    [ExcludeFromCodeCoverage]
    public class LayoutModel
    {

        public PreviewModel Preview { get; set; }

        public IEnumerable<InterimMenuItem> MenuItems { get; set; } = Enumerable.Empty<InterimMenuItem>();

        public BetaBanner? BetaBanner { get; set; }

        public InterimFooterLinks? InterimFooterLinks { get; set; }

        public LayoutModel(IEnumerable<InterimMenuItem> menuItems, BetaBanner? betaBanner, InterimFooterLinks? interimFooterLinks)
        {

            Preview = PreviewModel.NotPreviewModel;

            MenuItems = menuItems.Any() ? menuItems : Enumerable.Empty<InterimMenuItem>();

            BetaBanner = betaBanner;

            InterimFooterLinks = interimFooterLinks;

        }

        public LayoutModel()
        {

            Preview = PreviewModel.NotPreviewModel;

        }

    }

}
