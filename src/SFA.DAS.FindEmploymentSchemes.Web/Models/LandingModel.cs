using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{

    public class LandingModel : LayoutModel
    {

        public required string InterimLandingPageID { get; set; }

        public required string InterimLandingPageTitle { get; set; }

        public required InterimPreamble InterimLandingPagePreamble { get; set; }

        public List<InterimTileSection> InterimTileSections { get; set; } = [];

    }

}
