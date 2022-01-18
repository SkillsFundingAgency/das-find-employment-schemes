using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class FilterService : IFilterService
    {
        private readonly IContentService _contentService;
        private readonly ISchemesModelService _schemesModelService;

        public FilterService(
            IContentService contentService,
            ISchemesModelService schemesModelService)
        {
            _contentService = contentService;
            _schemesModelService = schemesModelService;
        }

        public HomeModel ApplyFilter(SchemeFilterViewModel filters)
        {
            var content = _contentService.Content;

            IEnumerable<Scheme> motivationSchemes =   filters.motivations.Any() ?
                                                            from Scheme s in content.Schemes
                                                            from string m in filters.motivations
                                                            where s.FilterAspects.Contains(m)
                                                            select s :
                                                            content.Schemes;
            IEnumerable<Scheme> schemeLengthSchemes = filters.schemeLength.Any() ?
                                                            from Scheme s in content.Schemes
                                                            from string l in filters.schemeLength
                                                            where s.FilterAspects.Contains(l)
                                                            select s :
                                                            content.Schemes;
            IEnumerable<Scheme> paySchemes =          filters.pay.Any() ?
                                                            from Scheme s in content.Schemes
                                                            from string p in filters.pay
                                                            where s.FilterAspects.Contains(p)
                                                            select s :
                                                            content.Schemes;
            IEnumerable<Scheme> filteredSchemes = (filters.allFilters.Any() ?
                                                      (from Scheme s in content.Schemes
                                                       join Scheme m in motivationSchemes
                                                       on s equals m
                                                       join Scheme l in schemeLengthSchemes
                                                       on s equals l
                                                       join Scheme p in paySchemes
                                                       on s equals p
                                                       select s
                                                      ).Distinct() :
                                                      content.Schemes);

            var filterGroupModels = new List<FilterGroupModel>
            {
                new FilterGroupModel(content.MotivationsFilter.Name, content.MotivationsFilter.Description,
                    content.MotivationsFilter.Aspects.Select(x =>
                        new FilterAspect(x.Id, x.Description, filters.motivations.Contains(x.Id)))),
                new FilterGroupModel(content.SchemeLengthFilter.Name, content.SchemeLengthFilter.Description,
                    content.SchemeLengthFilter.Aspects.Select(x => new FilterAspect(x.Id, x.Description, filters.schemeLength.Contains(x.Id)))),
                new FilterGroupModel(content.PayFilter.Name, content.PayFilter.Description,
                    content.PayFilter.Aspects.Select(x => new FilterAspect(x.Id, x.Description, filters.pay.Contains(x.Id))))
            };

            return new HomeModel(_schemesModelService.HomeModel.Preamble, filteredSchemes, filterGroupModels, true);
        }
    }
}
