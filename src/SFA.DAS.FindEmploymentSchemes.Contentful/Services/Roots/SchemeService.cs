using System.Collections.Generic;
using System.Linq;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Contentful.Core;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using ApiScheme = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.Scheme;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    public class SchemeService : ContentRootService
    {
        public SchemeService(HtmlRenderer htmlRenderer) : base(htmlRenderer)
        {
        }

        private async Task<IEnumerable<Scheme>> GetAll(IContentfulClient contentfulClient)
        {
            ContentfulCollection<ApiScheme> apiData = await GetSchemesApi(contentfulClient);
            return await SchemesToContent(apiData, contentfulClient.IsPreviewClient);
        }

        private Task<ContentfulCollection<ApiScheme>> GetSchemesApi(IContentfulClient contentfulClient)
        {
            var builder = QueryBuilder<ApiScheme>.New.ContentTypeIs("scheme").Include(1);
            return contentfulClient.GetEntries(builder);
        }

        //todo: IsValid method (in api scheme?)
        private Task<Scheme[]> SchemesToContent(ContentfulCollection<ApiScheme> schemes, bool includeInvalidContent = false)
        {
            return Task.WhenAll(schemes.Where(s => includeInvalidContent || (s != null && s?.Name != null && s?.ShortDescription != null && s?.ShortBenefits != null && s?.ShortTime != null && s?.Size != null && s?.Url != null))
                .OrderByDescending(s => s.Size)
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
                (await ToHtmlString(apiScheme.ShortDescription))!,
                (await ToHtmlString(apiScheme.ShortCost))!,
                (await ToHtmlString(apiScheme.ShortBenefits))!,
                (await ToHtmlString(apiScheme.ShortTime))!,
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
                subSchemes);
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
