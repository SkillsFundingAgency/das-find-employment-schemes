using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    public class InterimLandingPage
    {

        public required string InterimLandingPageID { get; set; }

        public required string InterimLandingPageTitle { get; set; }

        public required InterimPreamble InterimLandingPagePreamble { get; set; }

        public List<InterimTileSection> InterimTileSections { get; set; } = [];

    }

}
