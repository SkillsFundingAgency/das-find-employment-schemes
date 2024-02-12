using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Helpers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class FilterService : IFilterService
    {

        private readonly IContentService _contentService;

        private readonly ISchemesModelService _schemesModelService;

        private readonly ILogger<FilterService> _logger;

        public FilterService(

            IContentService contentService,

            ISchemesModelService schemesModelService,

            ILogger<FilterService> logger

        )
        {

            _contentService = contentService;

            _schemesModelService = schemesModelService;

            _logger = logger;

        }

        public HomeModel ApplyFilter(SchemeFilterModel filters)
        {
            var content = _contentService.Content;

            IEnumerable<Scheme> motivationSchemes =   filters.Motivations.Any() ?
                                                            from Scheme s in content.Schemes
                                                            from string m in filters.Motivations
                                                            where s.FilterAspects.Contains(m)
                                                            select s :
                                                            content.Schemes;
            IEnumerable<Scheme> schemeLengthSchemes = filters.SchemeLength.Any() ?
                                                            from Scheme s in content.Schemes
                                                            from string l in filters.SchemeLength
                                                            where s.FilterAspects.Contains(l)
                                                            select s :
                                                            content.Schemes;
            IEnumerable<Scheme> paySchemes =          filters.Pay.Any() ?
                                                            from Scheme s in content.Schemes
                                                            from string p in filters.Pay
                                                            where s.FilterAspects.Contains(p)
                                                            select s :
                                                            content.Schemes;
            IEnumerable<Scheme> filteredSchemes = (filters.AllFilters.Any() ?
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

            var filterGroupModels = new[]
            {
                new Filter(content.MotivationsFilter.Name, content.MotivationsFilter.Description,
                    content.MotivationsFilter.Aspects.Select(x =>
                        new FilterAspect(x.Id, x.Description, filters.Motivations.Contains(x.Id)))),
                new Filter(content.SchemeLengthFilter.Name, content.SchemeLengthFilter.Description,
                    content.SchemeLengthFilter.Aspects.Select(x => new FilterAspect(x.Id, x.Description, filters.SchemeLength.Contains(x.Id)))),
                new Filter(content.PayFilter.Name, content.PayFilter.Description,
                    content.PayFilter.Aspects.Select(x => new FilterAspect(x.Id, x.Description, filters.Pay.Contains(x.Id))))
            };

            return new HomeModel(
                
                _schemesModelService.HomeModel.Preamble, 
                
                filteredSchemes,
                
                filterGroupModels,

                content.MenuItems,
                
                true,

                filters.BuildFilterQueryString()

            );

        }

        /// <summary>
        /// Create an instance of the SchemeFilterModel class with pre-populated filter aspects.
        /// </summary>
        /// <param name="pay">The pay filter aspects concated into a comma separated string.</param>
        /// <param name="duration">The scheme length filter aspects concated into a comma separated string.</param>
        /// <param name="motivation">The motivation filter aspects concated into a comma separated string.</param>
        /// <returns>An instance of the SFA.DAS.FindEmploymentSchemes.Web.Models.SchemeFilterModel class.</returns>
        public SchemeFilterModel CreateFilterModel(string pay, string duration, string motivation)
        {

            try
            {

                return new SchemeFilterModel()
                {

                    Pay = StringHelper.SplitAndReturnList(pay, ','),

                    SchemeLength = StringHelper.SplitAndReturnList(duration, ','),

                    Motivations = StringHelper.SplitAndReturnList(motivation, ',')

                };

            }
            catch (Exception _exception)
            {

                _logger.LogError("Exception: {MethodName} - Unable to create an instance of {SchemeFilterModelClass}. [Exception: {ExceptionMessage}, InnerException: {InnerException}, StackTrace: {StackTrace}]",

                    nameof(CreateFilterModel),

                    nameof(SchemeFilterModel),

                    _exception.Message,

                    _exception.InnerException,

                    _exception.StackTrace

                );

                return new SchemeFilterModel();

            }

        }

        /// <summary>
        /// Remap a string conact version of each filter aspect into a new HomeModel instance.
        /// </summary>
        /// <param name="pay">The pay filter aspects concated into a comma separated string.</param>
        /// <param name="duration">The scheme length filter aspects concated into a comma separated string.</param>
        /// <param name="motivation">The motivation filter aspects concated into a comma separated string.</param>
        /// <returns>An instance of the SFA.DAS.FindEmploymentSchemes.Web.ModelsHomeModel class.</returns>
        public HomeModel RemapFilters(string pay, string duration, string motivation)
        {

            try
            {

                if (string.IsNullOrWhiteSpace(pay) && string.IsNullOrWhiteSpace(duration) && string.IsNullOrWhiteSpace(motivation))
                {

                    return _schemesModelService.HomeModel;

                }

                HomeModel remappedHomeModel = ApplyFilter(

                    CreateFilterModel(pay, duration, motivation)

                );

                return remappedHomeModel;

            }
            catch (Exception _exception)
            {

                _logger.LogError("Exception: {MethodName} - Unable to create an instance of {SchemeFilterModelClass}. [Exception: {ExceptionMessage}, InnerException: {InnerException}, StackTrace: {StackTrace}]",

                    nameof(RemapFilters),

                    nameof(SchemeFilterModel),

                    _exception.Message,

                    _exception.InnerException,

                    _exception.StackTrace

                );

                return _schemesModelService.HomeModel;

            }

        }

    }

}
