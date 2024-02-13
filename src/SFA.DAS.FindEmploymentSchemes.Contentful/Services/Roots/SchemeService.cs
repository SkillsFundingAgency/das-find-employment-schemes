﻿using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiScheme = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.Scheme;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    public class SchemeService : ContentRootService, ISchemeService
    {

        private readonly ILogger<SchemeService> _logger;

        public SchemeService(HtmlRenderer htmlRenderer, ILogger<SchemeService> logger) : base(htmlRenderer)
        {

            _logger = logger;

        }

        public async Task<IEnumerable<Scheme>> GetAll(IContentfulClient contentfulClient)
        {

            var builder = QueryBuilder<ApiScheme>.New.ContentTypeIs("scheme").Include(3);

            var schemes = await contentfulClient.GetEntries(builder);

            LogErrors(schemes);

            return await Task.WhenAll(
                
                    FilterValidUrl(schemes, _logger)

                .OrderBy(s => s.DefaultOrder)

                .Select(ToContent));

        }

        private async Task<Scheme> ToContent(ApiScheme apiScheme)
        {
            IEnumerable<CaseStudy> caseStudies = Enumerable.Empty<CaseStudy>();
            if (apiScheme.CaseStudyReferences != null)
            {
                caseStudies = await Task.WhenAll(apiScheme.CaseStudyReferences.Select(ToContent));
            }

            IEnumerable<SubScheme> subSchemes = Enumerable.Empty<SubScheme>();
            if (apiScheme.SubSchemes != null)
            {
                subSchemes = await Task.WhenAll(apiScheme.SubSchemes.Select(ToContent));
            }

            return new Scheme(
                apiScheme.Name!,
                apiScheme.ShortName!,
                apiScheme.VisitSchemeInformation!,
                (await ToHtmlString(apiScheme.ShortDescription))!,
                (await ToHtmlString(apiScheme.ShortCost))!,
                (await ToHtmlString(apiScheme.ShortBenefits))!,
                (await ToHtmlString(apiScheme.ShortTime))!,
                apiScheme.ComparisonRecruitOrTrain,
                apiScheme.ComparisonAgeCriteria,
                apiScheme.ComparisonCost,
                apiScheme.ComparisonDuration,
                apiScheme.Url!,
                apiScheme.Size,
                (apiScheme.PayFilterAspects?.Select(f => ToFilterAspectId(f, PayFilterService.Prefix)) ?? Enumerable.Empty<string>())
                    .Concat(apiScheme.MotivationsFilterAspects?.Select(f => ToFilterAspectId(f, MotivationFilterService.Prefix)) ?? Enumerable.Empty<string>())
                    .Concat(apiScheme.SchemeLengthFilterAspects?.Select(f => ToFilterAspectId(f, SchemeLengthFilterService.Prefix)) ?? Enumerable.Empty<string>()),
                caseStudies,
                await ToHtmlString(apiScheme.CaseStudies),
                await ToHtmlString(apiScheme.DetailsPageOverride),
                await ToHtmlString(apiScheme.Description),
                await ToHtmlString(apiScheme.Cost),
                await ToHtmlString(apiScheme.Responsibility),
                await ToHtmlString(apiScheme.Benefits),
                apiScheme.OfferHeader,
                await ToHtmlString(apiScheme.Offer),
                await ToHtmlString(apiScheme.AdditionalFooter),
                subSchemes,
                apiScheme.DefaultOrder,
                apiScheme.PopularityOrder,
                apiScheme.DurationOrder,
                apiScheme.CostOrder,
                apiScheme.Components.OrderBy(a => a.ComponentOrder ?? 0).ToList(),
                apiScheme.InterimPreamble,
                apiScheme.InterimBreadcrumbs,
                apiScheme.InterimTileSections
            );
        }

        private async Task<CaseStudy> ToContent(Model.Api.CaseStudy apiCaseStudy)
        {
            //todo: check all are mandatory in contentful
            return new CaseStudy(
                apiCaseStudy.Name!,
                apiCaseStudy.DisplayTitle!,
                (await ToHtmlString(apiCaseStudy.Description))!);
        }
        private async Task<SubScheme> ToContent(Model.Api.SubScheme apiSubScheme)
        {
            return new SubScheme(
                apiSubScheme.Title!,
                await ToHtmlString(apiSubScheme.Summary),
                (await ToHtmlString(apiSubScheme.Content))!);
        }
    }
}
