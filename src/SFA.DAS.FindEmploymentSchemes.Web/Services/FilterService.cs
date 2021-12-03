
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Web.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;


namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class FilterService : IFilterService
    {
        private const string MOTIVATION_NAME = "motivations";
        private const string MOTIVATION_DESCRIPTION = "I want to";
        private const string SCHEME_LENGTH_NAME = "schemeLength";
        private const string SCHEME_LENGTH_DESCRIPTION = "Length of scheme?";
        private const string PAY_NAME = "pay";
        private const string PAY_DESCRIPTION = "I can offer";

        public FilterGroupModel[] FilterGroupModels()
        {
            return new FilterGroupModel[] {
                new FilterGroupModel(MOTIVATION_NAME, MOTIVATION_DESCRIPTION, SchemesContent.MotivationsFilters),
                new FilterGroupModel(SCHEME_LENGTH_NAME, SCHEME_LENGTH_DESCRIPTION, SchemesContent.SchemeLengthFilters),
                new FilterGroupModel(PAY_NAME, PAY_DESCRIPTION, SchemesContent.PayFilters)
            };
        }

        public HomeModel HomeModel()
        {
            return new HomeModel(SchemesContent.Schemes, FilterGroupModels());
        }

        public IReadOnlyDictionary<string, SchemeDetailsModel> SchemeDetailsModels()
        {
            return BuildSchemeDetailsModelsDictionary();
        }

        public ReadOnlyDictionary<string, SchemeDetailsModel> BuildSchemeDetailsModelsDictionary()
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

            List<FilterGroupModel> filterGroupModels = new List<FilterGroupModel> { };
            filterGroupModels.Add(new FilterGroupModel(MOTIVATION_NAME, MOTIVATION_DESCRIPTION,
                                  SchemesContent.MotivationsFilters.Select(x => new MotivationsFilter(x.Id, x.Description, filters.motivations.Contains(x.Id)))));
            filterGroupModels.Add(new FilterGroupModel(SCHEME_LENGTH_NAME, SCHEME_LENGTH_DESCRIPTION,
                                  SchemesContent.SchemeLengthFilters.Select(x => new SchemeLengthFilter(x.Id, x.Description, filters.schemeLength.Contains(x.Id)))));
            filterGroupModels.Add(new FilterGroupModel(PAY_NAME, PAY_DESCRIPTION,
                                  SchemesContent.PayFilters.Select(x => new PayFilter(x.Id, x.Description, filters.pay.Contains(x.Id)))));

            return new HomeModel(filteredSchemes, filterGroupModels);
        }

    }
}
