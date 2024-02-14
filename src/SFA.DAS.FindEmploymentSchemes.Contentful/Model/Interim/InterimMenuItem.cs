using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    [ExcludeFromCodeCoverage]
    public class InterimMenuItem
    {

        public required string InterimMenuItemTitle { get; set; }

        public required string InterimMenuItemText { get; set; }

        public required string InterimMenuItemSource { get; set; }

        public required int InterimMenuItemOrder { get; set; }

    }

}
