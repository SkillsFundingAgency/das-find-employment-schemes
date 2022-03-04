using System.Collections.Generic;
using System.Linq;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Contentful.Core;
using System.Threading.Tasks;
using ContentScheme = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Scheme;
using ApiScheme = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.Scheme;
using Microsoft.AspNetCore.Html;
using System;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    public class ContentRootService
    {
        private readonly HtmlRenderer _htmlRenderer;

        public ContentRootService(HtmlRenderer htmlRenderer)
        {
            _htmlRenderer = htmlRenderer;
        }

        //this one might not belong here
        protected static string ToFilterAspectId(Model.Api.IFilter filter, string filterPrefix)
        {
            return $"{filterPrefix}--{Slugify(filter.Name)}";
        }

        protected static string Slugify(string? name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return name.ToLower().Replace(' ', '-');
        }

        protected async Task<HtmlString?> ToHtmlString(Document? document)
        {
            if (document == null)
                return null;

            string html = await _htmlRenderer.ToHtml(document);

            return ToNormalisedHtmlString(html);
        }

        /// <remarks>
        /// Should be private, but Contentful's .net library is not very test friendly (HtmlRenderer.ToHtml can't be mocked).
        /// We'd have to introduce a level of indirection to test this, were it private.
        /// </remarks>
        public static HtmlString ToNormalisedHtmlString(string html)
        {
            // replace left/right quotation marks with regular old quotation marks
            html = html.Replace('“', '"').Replace('”', '"');

            // sometimes contentful uses a \r and sometimes a \r\n - nice!
            // we could strip these out instead
            html = html.Replace("\r\n", "\r");
            html = html.Replace("\r", "\r\n");

            return new HtmlString(html);
        }
    }

    public class SchemeService : ContentRootService
    {
        public SchemeService(HtmlRenderer htmlRenderer) : base(htmlRenderer)
        {
        }

        private async Task<IEnumerable<ContentScheme>> Get(IContentfulClient contentfulClient)
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
        private Task<ContentScheme[]> SchemesToContent(ContentfulCollection<ApiScheme> schemes, bool includeInvalidContent = false)
        {
            return Task.WhenAll(schemes.Where(s => includeInvalidContent || (s != null && s?.Name != null && s?.ShortDescription != null && s?.ShortBenefits != null && s?.ShortTime != null && s?.Size != null && s?.Url != null))
                .OrderByDescending(s => s.Size)
                .Select(ToContent));
        }

        private async Task<ContentScheme> ToContent(ApiScheme apiScheme)
        {
            IEnumerable<Model.Content.CaseStudy> caseStudies = Enumerable.Empty<Model.Content.CaseStudy>();
            if (apiScheme.CaseStudyReferences != null)
            {
                caseStudies = await Task.WhenAll(apiScheme.CaseStudyReferences.Select(ToContent));
            }

            IEnumerable<Model.Content.SubScheme> subSchemes = Enumerable.Empty<Model.Content.SubScheme>();
            if (apiScheme.SubSchemes != null)
            {
                subSchemes = await Task.WhenAll(apiScheme.SubSchemes.Select(ToContent));
            }

            return new ContentScheme(
                apiScheme.Name!,
                (await ToHtmlString(apiScheme.ShortDescription))!,
                (await ToHtmlString(apiScheme.ShortCost))!,
                (await ToHtmlString(apiScheme.ShortBenefits))!,
                (await ToHtmlString(apiScheme.ShortTime))!,
                apiScheme.Url!,
                apiScheme.Size,
                (apiScheme.PayFilterAspects?.Select(f => ToFilterAspectId(f, PayFilterPrefix)) ?? Enumerable.Empty<string>())
                    .Concat(apiScheme.MotivationsFilterAspects?.Select(f => ToFilterAspectId(f, MotivationsFilterPrefix)) ?? Enumerable.Empty<string>())
                    .Concat(apiScheme.SchemeLengthFilterAspects?.Select(f => ToFilterAspectId(f, SchemeLengthFilterPrefix)) ?? Enumerable.Empty<string>()),
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

        private async Task<Model.Content.CaseStudy> ToContent(Model.Api.CaseStudy apiCaseStudy)
        {
            //todo: check all are mandatory in contentful
            return new Model.Content.CaseStudy(
                apiCaseStudy.Name!,
                apiCaseStudy.DisplayTitle!,
                (await ToHtmlString(apiCaseStudy.Description))!);
        }
        private async Task<Model.Content.SubScheme> ToContent(Model.Api.SubScheme apiSubScheme)
        {
            return new Model.Content.SubScheme(
                apiSubScheme.Title!,
                await ToHtmlString(apiSubScheme.Summary),
                (await ToHtmlString(apiSubScheme.Content))!);
        }
    }
}
