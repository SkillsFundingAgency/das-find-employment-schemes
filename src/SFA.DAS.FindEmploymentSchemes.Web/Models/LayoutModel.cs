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

        public LayoutModel(IEnumerable<InterimMenuItem> menuItems)
        {

            Preview = PreviewModel.NotPreviewModel;

            MenuItems = menuItems.Any() ? menuItems : Enumerable.Empty<InterimMenuItem>();

        }

        public LayoutModel()
        {

            Preview = PreviewModel.NotPreviewModel;

        }

    }

}
