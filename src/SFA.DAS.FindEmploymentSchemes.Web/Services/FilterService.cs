using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class FilterService : IFilterService
    {
        private const string HomepagePreambleUrl = "home";
        private const string MotivationName = "motivations";
        private const string MotivationDescription = "I want to";
        private const string SchemeLengthName = "schemeLength";
        private const string SchemeLengthDescription = "Length of scheme?";
        private const string PayName = "pay";
        private const string PayDescription = "I can offer";

        private static readonly HomeModel StaticHomeModel = new HomeModel(
            SchemesContent.Pages.First(p => p.Url == HomepagePreambleUrl).Content,
            SchemesContent.Schemes,
new[] {
                new FilterGroupModel(MotivationName, MotivationDescription, SchemesContent.MotivationsFilters),
                new FilterGroupModel(SchemeLengthName, SchemeLengthDescription, SchemesContent.SchemeLengthFilters),
                new FilterGroupModel(PayName, PayDescription, SchemesContent.PayFilters)
            });
        private static readonly ReadOnlyDictionary<string, SchemeDetailsModel> StaticSchemeDetailsModels = BuildSchemeDetailsModelsDictionary();

        public HomeModel HomeModel { get; set; } = StaticHomeModel;

        public IReadOnlyDictionary<string, SchemeDetailsModel> SchemeDetailsModels => StaticSchemeDetailsModels;

        private static ReadOnlyDictionary<string, SchemeDetailsModel> BuildSchemeDetailsModelsDictionary()
        {
            var schemeDetailsModels = new Dictionary<string, SchemeDetailsModel>();

            foreach (string schemeUrl in SchemesContent.Schemes.Select(s => s.Url))
            {
                schemeDetailsModels.Add(schemeUrl, new SchemeDetailsModel(schemeUrl, SchemesContent.Schemes));
            }

            return new ReadOnlyDictionary<string, SchemeDetailsModel>(schemeDetailsModels);
        }

        public HomeModel ApplyFilter(SchemeFilterViewModel filters)
        {
            IEnumerable<Scheme> motivationSchemes =   filters.motivations.Any() ?
                                                            from Scheme s in SchemesContent.Schemes
                                                            from string m in filters.motivations
                                                            where s.FilterAspects.Contains(m)
                                                            select s :
                                                      SchemesContent.Schemes;
            IEnumerable<Scheme> schemeLengthSchemes = filters.schemeLength.Any() ?
                                                            from Scheme s in SchemesContent.Schemes
                                                            from string l in filters.schemeLength
                                                            where s.FilterAspects.Contains(l)
                                                            select s :
                                                      SchemesContent.Schemes;
            IEnumerable<Scheme> paySchemes =          filters.pay.Any() ?
                                                            from Scheme s in SchemesContent.Schemes
                                                            from string p in filters.pay
                                                            where s.FilterAspects.Contains(p)
                                                            select s :
                                                      SchemesContent.Schemes;
            IEnumerable<Scheme> filteredSchemes = (filters.allFilters.Any() ?
                                                      (from Scheme s in SchemesContent.Schemes
                                                       join Scheme m in motivationSchemes
                                                       on s equals m
                                                       join Scheme l in schemeLengthSchemes
                                                       on s equals l
                                                       join Scheme p in paySchemes
                                                       on s equals p
                                                       select s
                                                      ).Distinct() :
                                                      SchemesContent.Schemes);

            var filterGroupModels = new List<FilterGroupModel>
            {
                new FilterGroupModel(MotivationName, MotivationDescription,
                    SchemesContent.MotivationsFilters.Select(x =>
                        new MotivationsFilter(x.Id, x.Description, filters.motivations.Contains(x.Id)))),
                new FilterGroupModel(SchemeLengthName, SchemeLengthDescription,
                    SchemesContent.SchemeLengthFilters.Select(x => new SchemeLengthFilter(x.Id, x.Description, filters.schemeLength.Contains(x.Id)))),
                new FilterGroupModel(PayName, PayDescription,
                    SchemesContent.PayFilters.Select(x => new PayFilter(x.Id, x.Description, filters.pay.Contains(x.Id))))
            };

            return new HomeModel(StaticHomeModel.Preamble, filteredSchemes, filterGroupModels, true);
        }
    }
}
