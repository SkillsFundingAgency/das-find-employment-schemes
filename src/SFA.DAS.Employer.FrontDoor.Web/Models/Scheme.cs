
namespace SFA.DAS.Employer.FrontDoor.Web.Models
{
    //todo: won't end up living here
    //todo: revisit for c#9
    public class Scheme
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string ShortCost { get; set; }
        public string ShortBenefits { get; set; }
        public string ShortTime { get; set; }
        public string Url { get; set; }
        public string? DetailsPageOverride { get; set; }
        public string? Description { get; set; }
        public string? Cost { get; set; }
        public string? Responsibility { get; set; }
        public string? Benefits { get; set; }
        public string? CaseStudies { get; set; }
        public string? OfferHeader { get; set; }
        public string? Offer { get; set; }
        // when we display the matching schemes on the filter page, we'll sort largest to smallest
        public int Size { get; set; }

        public Scheme(string name, string shortDescription, string shortCost, string shortBenefits, string shortTime, string url, int size,
            string? detailsPageOverride = null,
            string? description = null, string? cost = null, string? responsibility = null, string? benefits = null,
            string? caseStudies = null, string? offerHeader = null, string? offer = null)
        {
            Name = name;
            ShortDescription = shortDescription;
            ShortCost = shortCost;
            ShortBenefits = shortBenefits;
            ShortTime = shortTime;
            Url = url;
            Size = size;
            DetailsPageOverride = detailsPageOverride;
            Description = description;
            Cost = cost;
            Responsibility = responsibility;
            Benefits = benefits;
            CaseStudies = caseStudies;
            OfferHeader = offerHeader;
            Offer = offer;
        }
    }
}
