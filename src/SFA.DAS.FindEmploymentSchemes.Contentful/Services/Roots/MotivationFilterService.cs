using Contentful.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using Contentful.Core.Search;
using ApiFilter = SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api.Filter;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    public class FilterService : ContentRootService
    {
        private readonly string _name;
        private readonly string _description;
        private readonly string _contentfulTypeName;
        private readonly string _prefix;

        public FilterService(
            HtmlRenderer htmlRenderer,
            string name, string description, string contentfulTypeName, string prefix)
            : base(htmlRenderer)
        {
            _name = name;
            _description = description;
            _contentfulTypeName = contentfulTypeName;
            _prefix = prefix;
        }

        public async Task<Filter> Get(IContentfulClient contentfulClient)
        {
            return new Filter(
                _name,
                _description,
                await GetFilterAspects(contentfulClient));
        }

        private async Task<IEnumerable<FilterAspect>> GetFilterAspects(IContentfulClient contentfulClient)
        {
            var builder = QueryBuilder<ApiFilter>.New.ContentTypeIs(_contentfulTypeName);

            var filterAspects = await contentfulClient.GetEntries(builder);
            //todo
            //LogErrors(filterAspects);

            return filterAspects.OrderBy(f => f.Order).Select(ToContent);
        }

        private FilterAspect ToContent(Model.Api.IFilter apiFilter)
        {
            return new FilterAspect(ToFilterAspectId(apiFilter, _prefix), apiFilter.Description!);
        }
    }

    public class MotivationFilterService : FilterService
    {
        private const string Name = "motivations";
        private const string Description = "I want to";
        private const string ContentfulTypeName = "motivationsFilter";
        private const string Prefix = "motivations";

        public MotivationFilterService(HtmlRenderer htmlRenderer)
            : base(htmlRenderer, Name, Description, ContentfulTypeName, Prefix)
        {
        }
    }
}
