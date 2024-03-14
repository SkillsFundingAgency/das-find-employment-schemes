using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Helpers;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<HomeModel> ApplyFilter(SchemeFilterModel filters, bool isPreview = false)
        {

            if(isPreview)
            {

                await _contentService.UpdatePreview();

            }

            var content = isPreview ? _contentService.PreviewContent ?? _contentService.Content : _contentService.Content;

            var filterSections = content.SchemeFilters;

            List<Scheme> filteredSchemes = content.Schemes.ToList();

            if (filters.FilterAspects.Any())
            {

                foreach (SchemeFilter filterSection in filterSections.OrderBy(a => a.SchemeFilterOrder))
                {

                    // Get the selected aspects from the current filter section
                    string[] aspectsFromSection = filterSection.SchemeFilterAspects.Select(a => ToFilterAspectId(a)).ToArray();

                    string[] selectedAspectsFromSection = aspectsFromSection.Where(a => filters.FilterAspects.Contains(a)).ToArray();

                    if(aspectsFromSection.Length == selectedAspectsFromSection.Length)
                    {

                        continue;

                    }

                    if(selectedAspectsFromSection.Any())
                    {

                        // Filter schemes based on the selected aspects from the current filter section
                        var filteredSchemesFromSection = from Scheme s in filteredSchemes
                                                         from string m in selectedAspectsFromSection
                                                         where s.FilterAspects.Contains(m)
                                                         select s;

                        // Add the filtered schemes from the current filter section to the result
                        filteredSchemes = filteredSchemesFromSection.Distinct().ToList();

                    }

                }

            }

            return new HomeModel(
                
                filteredSchemes.Distinct(),

                _schemesModelService.GetFilterSections(content.SchemeFilters, filters),

                content.MenuItems,

                _schemesModelService.HomeModel.InterimPreamble,

                _schemesModelService.HomeModel.InterimPageBreadcrumbs,

                content.BetaBanner,

                content.InterimFooterLinks,
                
                true,

                filters.BuildFilterQueryString()

            );

        }

        private static string ToFilterAspectId(SchemeFilterAspect filterAspect)
        {
            return $"{filterAspect.SchemeFilterAspectPrefix}--{Slugify(filterAspect.SchemeFilterAspectName)}";
        }

        private static string Slugify(string? name)
        {
            ArgumentNullException.ThrowIfNull(name);

            return name.ToLower().Replace(' ', '-');
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
        /// Remap a string concated version of each filter aspect into a new HomeModel instance.
        /// </summary>
        /// <param name="pay">The pay filter aspects concated into a comma separated string.</param>
        /// <param name="duration">The scheme length filter aspects concated into a comma separated string.</param>
        /// <param name="motivation">The motivation filter aspects concated into a comma separated string.</param>
        /// <returns>An instance of the SFA.DAS.FindEmploymentSchemes.Web.ModelsHomeModel class.</returns>
        public async Task<HomeModel> RemapFilters(string filters, bool isPreview = false)
        {

            try
            {

                if (string.IsNullOrWhiteSpace(filters))
                {

                    if(isPreview)
                    {

                        return await _schemesModelService.CreateHomeModelPreview();

                    }
                    else
                    {

                        return _schemesModelService.HomeModel;

                    }

                }

                HomeModel remappedHomeModel = await ApplyFilter(

                    CreateFilterModel(filters),

                    isPreview

                );

                if(isPreview)
                {

                    remappedHomeModel.Preview = new PreviewModel(Enumerable.Empty<HtmlString>());

                }

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
