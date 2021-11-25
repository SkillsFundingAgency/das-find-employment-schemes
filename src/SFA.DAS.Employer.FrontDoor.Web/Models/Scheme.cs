
using Microsoft.AspNetCore.Html;

namespace SFA.DAS.Employer.FrontDoor.Web.Models
{
    //todo: won't end up living here
    //todo: revisit for c#9
    public class Scheme
    {
        public string Name { get; set; }
        public IHtmlContent ShortDescription { get; set; }
        public IHtmlContent ShortCost { get; set; }
        public IHtmlContent ShortBenefits { get; set; }
        public IHtmlContent ShortTime { get; set; }
        public string Url { get; set; }
        public IHtmlContent? DetailsPageOverride { get; set; }
        public IHtmlContent? Description { get; set; }
        public IHtmlContent? Cost { get; set; }
        public IHtmlContent? Responsibility { get; set; }
        public IHtmlContent? Benefits { get; set; }
        public IHtmlContent? CaseStudies { get; set; }
        public string? OfferHeader { get; set; }
        public IHtmlContent? Offer { get; set; }
        // when we display the matching schemes on the filter page, we'll sort largest to smallest
        public int Size { get; set; }

        public Scheme(string name, IHtmlContent shortDescription, IHtmlContent shortCost, IHtmlContent shortBenefits, IHtmlContent shortTime,
            string url, int size,
            IHtmlContent? detailsPageOverride = null,
            IHtmlContent? description = null, IHtmlContent? cost = null, IHtmlContent? responsibility = null, IHtmlContent? benefits = null,
            IHtmlContent? caseStudies = null, string? offerHeader = null, IHtmlContent? offer = null)
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
