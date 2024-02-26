using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class SchemesModelService : ISchemesModelService
    {

        private const string HomepagePreambleUrl = "home";

        private readonly IContentService _contentService;

        private IReadOnlyDictionary<string, SchemeDetailsModel> _schemeDetailsModels;

        public HomeModel HomeModel { get; set; }

        public ComparisonModel ComparisonModel { get; private set; }

        private readonly ILogger<SchemesModelService> _logger;

#pragma warning disable CS8618
        public SchemesModelService(ILogger<SchemesModelService> logger, IContentService contentService)
        {

            _logger = logger;

            _contentService = contentService;

            contentService.ContentUpdated += OnContentUpdated;

            BuildModels();

        }
        #pragma warning restore CS8618

        // tried https://github.com/manuelroemer/Nullable (as we're using a legacy lts release, but didn't work)
        //[MemberNotNull(nameof(HomeModel))]
        //[MemberNotNull(nameof(SchemeDetailsModels))]
        private void BuildModels()
        {

            HomeModel = CreateHomeModel(_contentService.Content);

            ComparisonModel = CreateComparisonModel(_contentService.Content);

            _schemeDetailsModels = BuildSchemeDetailsModelsDictionary();

        }

        private void OnContentUpdated(object? sender, EventArgs args)
        {
            BuildModels();
        }

        private HomeModel CreateHomeModel(IContent content)
        {

            Page? page = content.Pages.FirstOrDefault(p => p.Url == HomepagePreambleUrl);

            return new HomeModel(

                content.Schemes,

                GetFilterSections(content.SchemeFilters),

                content.MenuItems,

                page?.InterimPreamble,

                page?.InterimBreadcrumbs,

                content.BetaBanner,

                content.InterimFooterLinks

            );

        }

        private ComparisonModel CreateComparisonModel(IContent content)
        {

            return new ComparisonModel(
                
                content.SchemeComparison, 
                
                content.Schemes, 
                
                content.MenuItems, 
                
                content.BetaBanner
                
            );

        }

        private ComparisonResultsModel CreateComparisonResultsModel(IEnumerable<string> schemes, SchemeFilterModel filters, IContent content)
        {

            if(schemes.Any())
            {

                return new ComparisonResultsModel(

                    content.SchemeComparison,
                
                    content.Schemes.Where(x => schemes.Contains(x.HtmlId)),

                    filters,

                    content.MenuItems,

                    content.BetaBanner,

                    content.InterimFooterLinks

                );

            }
            else
            {

                return new ComparisonResultsModel(

                    content.SchemeComparison,

                    content.Schemes,

                    filters,

                    content.MenuItems,

                    content.BetaBanner,

                    content.InterimFooterLinks

                );

            }

        }

        public ComparisonResultsModel CreateComparisonResultsModel(IEnumerable<string> schemes, SchemeFilterModel filters)
        {

            return CreateComparisonResultsModel(

                schemes,

                filters,
                
                _contentService.Content

            );

        }

        public async Task<HomeModel> CreateHomeModelPreview()
        {
            IContent previewContent = await _contentService.UpdatePreview();

            var homeModel = CreateHomeModel(previewContent);
            homeModel.Preview = new PreviewModel(GetHomeErrors(homeModel));

            return homeModel;
        }

        public async Task<ComparisonModel> CreateComparisonModelPreview()
        {
            IContent previewContent = await _contentService.UpdatePreview();

            var comparisonModel = CreateComparisonModel(previewContent);
            comparisonModel.Preview = new PreviewModel(Enumerable.Empty<HtmlString>());

            return comparisonModel;
        }

        public async Task<ComparisonResultsModel> CreateComparisonResultsModelPreview(IEnumerable<string> schemes, SchemeFilterModel filters)
        {
            IContent previewContent = await _contentService.UpdatePreview();

            var comparisonResultsModel = CreateComparisonResultsModel(schemes, filters, previewContent);
            comparisonResultsModel.Preview = new PreviewModel(Enumerable.Empty<HtmlString>());

            return comparisonResultsModel;
        }

        private ReadOnlyDictionary<string, SchemeDetailsModel> BuildSchemeDetailsModelsDictionary()
        {
            var schemeDetailsModels = new Dictionary<string, SchemeDetailsModel>();

            foreach (string schemeUrl in _contentService.Content.Schemes.Select(s => s.Url))
            {
                schemeDetailsModels.Add(
                    
                    schemeUrl, 
                    
                    new SchemeDetailsModel(
                        
                        schemeUrl, 
                        
                        _contentService.Content.Schemes,

                        _contentService.Content.MenuItems,

                         _contentService.Content.BetaBanner,

                        _contentService.Content.InterimFooterLinks

                    )
                    
                );

            }

            return new ReadOnlyDictionary<string, SchemeDetailsModel>(schemeDetailsModels);
        }

        public SchemeDetailsModel? GetSchemeDetailsModel(string schemeUrl)
        {
            _schemeDetailsModels.TryGetValue(schemeUrl, out SchemeDetailsModel? schemeDetailsModel);
            return schemeDetailsModel;
        }

        public async Task<SchemeDetailsModel?> GetSchemeDetailsModelPreview(string schemeUrl)
        {
            IContent previewContent = await _contentService.UpdatePreview();

            try
            {
                var model = new SchemeDetailsModel(schemeUrl, previewContent.Schemes, previewContent.MenuItems, previewContent.BetaBanner, previewContent.InterimFooterLinks);
                model.Preview = new PreviewModel(GetSchemeDetailsErrors(model));
                return model;
            }
            catch (Exception)
            {
                return default;
            }
        }

        private IEnumerable<HtmlString> GetHomeErrors(HomeModel model)
        {
            var errors = new List<HtmlString>();

            if (model.InterimPreamble == null)
            {
                errors.Add(new HtmlString("Preamble must not be blank"));
            }

            return errors;
        }

        private IEnumerable<HtmlString> GetSchemeDetailsErrors(SchemeDetailsModel model)
        {
            var errors = new List<HtmlString>();

            if (string.IsNullOrWhiteSpace(model.Scheme.Name))
            {
                errors.Add(new HtmlString("Name must not be blank"));
            }
            if (model.Scheme.ShortDescription == null)
            {
                errors.Add(new HtmlString("Short description must not be blank"));
            }
            if (model.Scheme.ShortCost == null)
            {
                errors.Add(new HtmlString("Short cost must not be blank"));
            }
            if (model.Scheme.ShortBenefits == null)
            {
                errors.Add(new HtmlString("Short benefits must not be blank"));
            }
            if (model.Scheme.ShortTime == null)
            {
                errors.Add(new HtmlString("Short time must not be blank"));
            }

            if (model.Scheme.DetailsPageOverride != null)
                return errors;

            if (model.Scheme.SubSchemes.Any())
                return errors;

            if (model.Scheme.OfferHeader == null)
            {
                errors.Add(new HtmlString("If there is no details page override and no sub schemes, the offer header must not be blank"));
            }

            return errors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemeFilters"></param>
        /// <returns></returns>
        public List<FilterSectionModel> GetFilterSections(List<SchemeFilter> schemeFilters, SchemeFilterModel? model = null)
        {

            _logger.LogInformation("Beginning {MethodName}", nameof(GetFilterSections));

            try
            {

                List<FilterSectionModel> filterModels = new List<FilterSectionModel>();

                foreach (SchemeFilter schemeFilter in schemeFilters.OrderBy(a => a.SchemeFilterOrder))
                {

                    FilterSectionModel sectionModel = new FilterSectionModel()
                    {



                        FilterSectionModelID = ToConvertedID(schemeFilter.SchemeFilterPrefix, schemeFilter.SchemeFilterDescription),

                        FilterSectionModelName = schemeFilter.SchemeFilterDescription,

                        FilterSectionModelAspects = schemeFilter.SchemeFilterAspects.Select(a =>

                        {

                            string aspectID = ToConvertedID(a.SchemeFilterAspectPrefix, a.SchemeFilterAspectName);

                            return new FilterSectionAspectModel()
                            {

                                FilterSectionAspectModelID = aspectID,

                                FilterSectionAspectDisplayName = a.SchemeFilterAspectName,

                                FilterSectionAspectSelected = model != null && model.FilterAspects.Any() && model.FilterAspects.Contains(aspectID)

                            };

                        }

                        ).ToList()

                    };

                    filterModels.Add(sectionModel);

                }

                return filterModels;

            }
            catch (Exception _exception)
            {

                _logger.LogError(_exception, "Unable to build out filter section models");

                return new List<FilterSectionModel>();

            }

        }

        private string ToConvertedID(string prefix, string postfix)
        {

            return $"{prefix}--{Slugify(postfix)}";

        }

        private string Slugify(string name)
        {

            return name.ToLower().Replace(' ', '-');

        }

    }
}
