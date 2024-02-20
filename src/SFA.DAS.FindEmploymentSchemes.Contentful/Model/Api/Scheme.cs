using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api
{
    [DebuggerDisplay("{Name}")]
    [ExcludeFromCodeCoverage]
    public class Scheme : IRootContent
    {

        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? VisitSchemeInformation { get; set; }
        public Document? ShortDescription { get; set; }
        public required string ShortCostHeading { get; set; }
        public required string ShortBenefitsHeading { get; set; }
        public required string ShortTimeHeading { get; set; }
        public Document? ShortCost { get; set; }
        public Document? ShortBenefits { get; set; }
        public Document? ShortTime { get; set; }
        public string? ComparisonRecruitOrTrain { get; set; }
        public string? ComparisonAgeCriteria { get; set; }
        public string? ComparisonCost { get; set; }
        public string? ComparisonDuration { get; set; }
        public string? Url { get; set; }
        public int Size { get; set; }
        // optional
        public Document? DetailsPageOverride { get; set; }
        public List<SubScheme>? SubSchemes { get; set; }
        public Document? CaseStudies { get; set; }
        public List<CaseStudy>? CaseStudyReferences { get; set; }
        public string? OfferHeader { get; set; }
        public Document? AdditionalFooter { get; set; }
        public List<SchemeFilterAspect> SchemeFilterAspects { get; set; } = [];

        public int? DefaultOrder { get; set; }

        public int? PopularityOrder { get; set; }

        public int? DurationOrder { get; set; }

        public int? CostOrder { get; set; }

        public List<InterimPageComponent> Components { get; set; } = [];

        public InterimPreamble? InterimPreamble { get; set; }

        public List<InterimTileSection>? InterimTileSections { get; set; } = [];

        public InterimBreadcrumbs? InterimBreadcrumbs { get; set; }

    }

}
