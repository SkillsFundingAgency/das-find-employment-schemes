using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    [ExcludeFromCodeCoverage]
    public class InterimBreadcrumbs
    {

        public required string InterimBreadcrumbTitle { get; set; }

        public List<InterimBreadcrumbLink> InterimBreadcrumLinks { get; set; } = [];

    }

}
