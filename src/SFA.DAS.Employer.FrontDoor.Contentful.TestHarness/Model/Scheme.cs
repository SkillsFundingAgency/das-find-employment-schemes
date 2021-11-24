using Contentful.Core.Models;

namespace SFA.DAS.Employer.FrontDoor.Contentful.TestHarness.Model
{
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
        //todo: add size to contentful
        //public int Size { get; set; }
    }
}
