using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    [ExcludeFromCodeCoverage]
    public class InterimBreadcrumbLink
    {

        public required string InterimBreadcrumbLinkTitle { get; set; }

        public required string InterimBreadcrumbLinkText { get; set; }

        public required string? InterimBreadcrumbLinkSource { get; set; }

        public required bool InterimBreadcrumbLinkActive { get; set; }
        
        public required int InterimBreadcrumbLinkOrder { get; set; }

    }

}
