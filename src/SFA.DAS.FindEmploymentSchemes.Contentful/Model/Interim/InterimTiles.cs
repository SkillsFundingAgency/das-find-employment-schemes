using Contentful.Core.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    [ExcludeFromCodeCoverage]
    public class InterimTileSection
    {

        public required string InterimTileSectionTitle { get; set; }

        public string? InterimTileSectionHeading { get; set; }

        public Document? InterimTileSectionDescription { get; set; }

        public List<InterimTile> InterimTiles { get; set; } = [];

    }

}
