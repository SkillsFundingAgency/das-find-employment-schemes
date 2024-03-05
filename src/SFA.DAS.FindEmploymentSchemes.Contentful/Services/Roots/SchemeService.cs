using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using System;
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

            try
            {

                var builder = QueryBuilder<ApiScheme>.New.ContentTypeIs("scheme").Include(3);

                var schemes = await contentfulClient.GetEntries(builder);

                LogErrors(schemes);

                return await Task.WhenAll(

                        FilterValidUrl(schemes, _logger)

                    .OrderBy(s => s.DefaultOrder)

                    .Select(ToContent));

            }
            catch(Exception _exception)
            {

                _logger.LogError(_exception, "Unable to get schemes from contentful.");

                return Enumerable.Empty<Scheme>();

            }

        }

        /// <summary>
        /// Retrieves the scheme comparison model from the Contentful CMS using the provided Contentful client.
        /// </summary>
        /// <param name="contentfulClient">The Contentful client used to retrieve data from the CMS.</param>
        /// <returns>Returns the scheme comparison model if available, otherwise returns null.</returns>
        public async Task<SchemeComparison?> GetSchemeComparison(IContentfulClient contentfulClient)
        {

            _logger.LogInformation("Beginning {MethodName}...", nameof(GetSchemeComparison));

            try
            {

                var query = new QueryBuilder<SchemeComparison>()

                    .ContentTypeIs("schemeComparison")

                    .Include(3);

                var results = await contentfulClient.GetEntries(query);

                List<SchemeComparison> resultList = results.Items.ToList();

                if (resultList.Any())
                {

                    SchemeComparison schemeComparison = resultList[0];

                    _logger.LogInformation("Retrieved scheme comparison: {Title}", schemeComparison.SchemeComparisonTitle);

                    return schemeComparison;

                }
                else
                {

                    _logger.LogInformation("No matching scheme comparison found.");

                    return null;

                }

            }
            catch (Exception _Exception)
            {

                _logger.LogError(_Exception, "Unable to get the scheme comparison model.");

                return null;

            }

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
                apiScheme.ShortBenefitsHeading,
                apiScheme.ShortCostHeading,
                apiScheme.ShortTimeHeading,
                (await ToHtmlString(apiScheme.ShortCost))!,
                (await ToHtmlString(apiScheme.ShortBenefits))!,
                (await ToHtmlString(apiScheme.ShortTime))!,
                apiScheme.ComparisonRecruitOrTrain,
                apiScheme.ComparisonAgeCriteria,
                apiScheme.ComparisonCost,
                apiScheme.ComparisonDuration,
                apiScheme.Url!,
                apiScheme.Size,
                apiScheme.SchemeFilterAspects,
                apiScheme.SchemeFilterAspects?.Select(f => ToFilterAspectId(f)) ?? new List<string>(),
                caseStudies,
                await ToHtmlString(apiScheme.CaseStudies),
                await ToHtmlString(apiScheme.DetailsPageOverride),
                apiScheme.OfferHeader,
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
