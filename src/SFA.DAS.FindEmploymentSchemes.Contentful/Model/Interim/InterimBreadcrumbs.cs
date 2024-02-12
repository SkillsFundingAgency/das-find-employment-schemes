using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    public class InterimBreadcrumbs
    {

        public required string InterimBreadcrumbTitle { get; set; }

        public List<InterimBreadcrumbLink> InterimBreadcrumLinks { get; set; } = [];

    }

}
