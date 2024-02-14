using Contentful.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    [ExcludeFromCodeCoverage]
    public class InterimTile
    {

        public required string InterimTileID { get; set; }

        public required string InterimTileTitle { get; set; }

        public required string InterimTileSource { get; set; }

        public required string InterimTileImageSource { get; set; }

        public required string InterimTileHeading { get; set; }

        public Document? InterimTileDescription { get; set; }

        public int? InterimTileOrder { get; set; } = 0;

    }

}
