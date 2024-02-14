using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    [ExcludeFromCodeCoverage]
    public class InterimPage
    {

        public required string InterimPageTitle { get; set; }

        public required string InterimPageURL { get; set; }

        public InterimPreamble? InterimPagePreamble { get; set; }

        public InterimBreadcrumbs? InterimPageBreadcrumbs { get; set; }

        public List<InterimPageComponent> InterimPageComponents { get; set; } = [];

        public List<InterimTileSection> InterimPageTileSections { get; set; } = [];

    }

}
