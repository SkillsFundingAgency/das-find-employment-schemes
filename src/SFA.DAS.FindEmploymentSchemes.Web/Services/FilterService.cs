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

        //private const string HomepagePreambleUrl = "home";
        //todo: these don't belong here
        private const string MotivationName = "motivations";
        private const string MotivationDescription = "I want to";
        private const string SchemeLengthName = "schemeLength";
        private const string SchemeLengthDescription = "Length of scheme?";
        private const string PayName = "pay";
        private const string PayDescription = "I can offer";

        public FilterService(
            IContentService contentService,
            ISchemesModelService schemesModelService)
        {
            _contentService = contentService;
            _schemesModelService = schemesModelService;
        }

        //        private static readonly HomeModel StaticHomeModel = new HomeModel(
//            GeneratedContent.Pages.First(p => p.Url == HomepagePreambleUrl).Content,
//            GeneratedContent.Schemes,
//new[] {
//                new FilterGroupModel(MotivationName, MotivationDescription, GeneratedContent.MotivationsFilters),
//                new FilterGroupModel(SchemeLengthName, SchemeLengthDescription, GeneratedContent.SchemeLengthFilters),
//                new FilterGroupModel(PayName, PayDescription, GeneratedContent.PayFilters)
//            });
//        private static readonly ReadOnlyDictionary<string, SchemeDetailsModel> StaticSchemeDetailsModels = BuildSchemeDetailsModelsDictionary();

//        public HomeModel HomeModel { get; set; } = StaticHomeModel;

//        public IReadOnlyDictionary<string, SchemeDetailsModel> SchemeDetailsModels => StaticSchemeDetailsModels;

//        private static ReadOnlyDictionary<string, SchemeDetailsModel> BuildSchemeDetailsModelsDictionary()
//        {
//            var schemeDetailsModels = new Dictionary<string, SchemeDetailsModel>();

//            foreach (string schemeUrl in GeneratedContent.Schemes.Select(s => s.Url))
//            {
//                schemeDetailsModels.Add(schemeUrl, new SchemeDetailsModel(schemeUrl, GeneratedContent.Schemes));
//            }

//            return new ReadOnlyDictionary<string, SchemeDetailsModel>(schemeDetailsModels);
//        }

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
                new FilterGroupModel(MotivationName, MotivationDescription,
                    content.MotivationsFilters.Select(x =>
                        new MotivationsFilter(x.Id, x.Description, filters.motivations.Contains(x.Id)))),
                new FilterGroupModel(SchemeLengthName, SchemeLengthDescription,
                    content.SchemeLengthFilters.Select(x => new SchemeLengthFilter(x.Id, x.Description, filters.schemeLength.Contains(x.Id)))),
                new FilterGroupModel(PayName, PayDescription,
                    content.PayFilters.Select(x => new PayFilter(x.Id, x.Description, filters.pay.Contains(x.Id))))
            };

            return new HomeModel(_schemesModelService.HomeModel.Preamble, filteredSchemes, filterGroupModels, content, true);
        }
    }
}
