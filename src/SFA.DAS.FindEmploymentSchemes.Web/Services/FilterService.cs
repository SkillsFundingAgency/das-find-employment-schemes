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

            List<Scheme> filteredSchemes = new List<Scheme>(); 

            if(!filters.FilterAspects.Any())
            {

                filteredSchemes = content.Schemes.ToList();

            }
            else
            {

                filteredSchemes = (filters.FilterAspects.Any() ?
                                    from Scheme s in content.Schemes
                                    from string m in filters.FilterAspects
                                    where s.FilterAspects.Contains(m)
                                    select s :
                                    content.Schemes).Distinct().ToList();

            }
           
            return new HomeModel(
                
                _schemesModelService.HomeModel.Preamble,
                
                filteredSchemes,

                _schemesModelService.GetFilterSections(content.SchemeFilters, filters),

                content.MenuItems,

                content.BetaBanner,
                
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
        public SchemeFilterModel CreateFilterModel(string filters)
        {

            try
            {

                return new SchemeFilterModel()
                {

                    FilterAspects = StringHelper.SplitAndReturnList(filters, ',')

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
        public HomeModel RemapFilters(string filters)
        {

            try
            {

                if (string.IsNullOrWhiteSpace(filters))
                {

                    return _schemesModelService.HomeModel;

                }

                HomeModel remappedHomeModel = ApplyFilter(

                    CreateFilterModel(filters)

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
