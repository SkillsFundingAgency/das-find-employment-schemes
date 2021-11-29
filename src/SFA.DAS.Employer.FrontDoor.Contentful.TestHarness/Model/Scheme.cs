using System.Collections.Generic;
using Contentful.Core.Models;
using System.Diagnostics;

namespace SFA.DAS.Employer.FrontDoor.Contentful.TestHarness.Model
{
    [DebuggerDisplay("{Name}")]
    public class Scheme
    {
        public string Name { get; set; }
        public Document ShortDescription { get; set; }
        public Document ShortCost { get; set; }
        public Document ShortBenefits { get; set; }
        public Document ShortTime { get; set; }
        public string Url { get; set; }
        public Document? DetailsPageOverride { get; set; }
        public Document? Description { get; set; }
        public Document? Cost { get; set; }
        public Document? Responsibility { get; set; }
        public Document? Benefits { get; set; }
        public Document? CaseStudies { get; set; }
        public string? OfferHeader { get; set; }
        public Document? Offer { get; set; }
        public int Size { get; set; }

        public List<MotivationsFilter> MotivationsFilters { get; set; }
        public List<PayFilter> PayFilters { get; set; }
        public List<SchemeLengthFilter> SchemeLengthFilters { get; set; }
    }
}
