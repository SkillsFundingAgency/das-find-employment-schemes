﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    //todo: interface?
    //todo: revisit for c#9
    public class Scheme
    {
        public string Name { get; }
        public HtmlString ShortDescription { get; }
        public HtmlString ShortCost { get; }
        public HtmlString ShortBenefits { get; }
        public HtmlString ShortTime { get; }
        public string Url { get; }
        public HtmlString? DetailsPageOverride { get; }
        public HtmlString? Description { get; }
        public IEnumerable<SubScheme> SubSchemes { get; }
        public HtmlString? Cost { get; }
        public HtmlString? Responsibility { get; }
        public HtmlString? Benefits { get; }
        public HtmlString? CaseStudies { get; }
        public string? OfferHeader { get; }
        public HtmlString? Offer { get; }
        public HtmlString? AdditionalFooter { get; }
        // when we display the matching schemes on the filter page, we'll sort largest to smallest
        public int Size { get; }
        public IEnumerable<string> FilterAspects { get; }
        public string HtmlId { get; }

        public Scheme(string name, HtmlString shortDescription, HtmlString shortCost, HtmlString shortBenefits, HtmlString shortTime,
            string url, int size,
            IEnumerable<string> filterAspects,
            HtmlString? detailsPageOverride = null,
            HtmlString? description = null, HtmlString? cost = null, HtmlString? responsibility = null, HtmlString? benefits = null,
            HtmlString? caseStudies = null, string? offerHeader = null, HtmlString? offer = null, HtmlString? additionalFooter = null,
            IEnumerable<SubScheme>? subSchemes = null)
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
            SubSchemes = subSchemes ?? Enumerable.Empty<SubScheme>();
            Description = description;
            Cost = cost;
            Responsibility = responsibility;
            Benefits = benefits;
            CaseStudies = caseStudies;
            OfferHeader = offerHeader;
            Offer = offer;
            AdditionalFooter = additionalFooter;

            HtmlId = SanitizeHtmlId(url);
            if (HtmlId == "")
                throw new ArgumentException("Must sanitize to a valid HTML id", nameof(url));
        }

        private string SanitizeHtmlId(string unsanitizedId)
        {
            //todo: now run online, so we could compile (although not on a fast path)

            // strip invalid chars
            string sanitizedHtmlId = Regex.Replace(unsanitizedId, @"[^a-zA-Z0-9-_:\.]", "");

            // ensure starts with a letter
            return Regex.Replace(sanitizedHtmlId, @"^[^a-zA-Z]*", "");
        }
    }
}
