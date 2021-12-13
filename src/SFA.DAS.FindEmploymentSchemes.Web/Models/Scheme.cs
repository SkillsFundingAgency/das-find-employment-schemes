using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;
using System;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    //todo: won't end up living here
    //todo: revisit for c#9
    public class Scheme
    {
        public string Name { get; }
        public IHtmlContent ShortDescription { get; }
        public IHtmlContent ShortCost { get; }
        public IHtmlContent ShortBenefits { get; }
        public IHtmlContent ShortTime { get; }
        public string Url { get; }
        public IHtmlContent? DetailsPageOverride { get; }
        public IHtmlContent? Description { get; }
        public IHtmlContent? Cost { get; }
        public IHtmlContent? Responsibility { get; }
        public IHtmlContent? Benefits { get; }
        public IHtmlContent? CaseStudies { get; }
        public string? OfferHeader { get; }
        public IHtmlContent? Offer { get; }
        // when we display the matching schemes on the filter page, we'll sort largest to smallest
        public int Size { get; }
        public string[] FilterAspects { get; }
        public string HtmlId { get; }

        public Scheme(string name, IHtmlContent shortDescription, IHtmlContent shortCost, IHtmlContent shortBenefits, IHtmlContent shortTime,
            string url, int size,
            string[] filterAspects,
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
            FilterAspects = filterAspects;
            DetailsPageOverride = detailsPageOverride;
            Description = description;
            Cost = cost;
            Responsibility = responsibility;
            Benefits = benefits;
            CaseStudies = caseStudies;
            OfferHeader = offerHeader;
            Offer = offer;

            HtmlId = SanitizeHtmlId(url);
            if (HtmlId == "")
                throw new ArgumentException("Must sanitize to a valid HTML id", nameof(url));
        }

        private string SanitizeHtmlId(string unsanitizedId)
        {
            // only run at startup, so we don't compile

            // strip invalid chars
            string sanitizedHtmlId = Regex.Replace(unsanitizedId, @"[^a-zA-Z0-9-_:\.]", "");

            // ensure starts with a letter
            return Regex.Replace(sanitizedHtmlId, @"^[^a-zA-Z]*", "");
        }
    }
}
