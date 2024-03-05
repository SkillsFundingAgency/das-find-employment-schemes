using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    [ExcludeFromCodeCoverage]
    public class InterimLandingPage
    {

        public required string InterimLandingPageID { get; set; }

        public required string InterimLandingPageTitle { get; set; }

        public required InterimPreamble InterimLandingPagePreamble { get; set; }

        public List<InterimTileSection> InterimTileSections { get; set; } = [];

    }

}
