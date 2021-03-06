using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class SchemesModelService : ISchemesModelService
    {
        private const string HomepagePreambleUrl = "home";

        private readonly IContentService _contentService;
        private IReadOnlyDictionary<string, SchemeDetailsModel> _schemeDetailsModels;

        public HomeModel HomeModel { get; private set; }
        public ComparisonModel ComparisonModel { get; private set; }

#pragma warning disable CS8618
        public SchemesModelService(IContentService contentService)
        {
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
            return new HomeModel(
                content.Pages.First(p => p.Url == HomepagePreambleUrl).Content,
                content.Schemes,
                new[] { content.MotivationsFilter, content.SchemeLengthFilter, content.PayFilter });
        }

        private ComparisonModel CreateComparisonModel(IContent content)
        {
            return new ComparisonModel(content.Schemes);
        }

        private ComparisonResultsModel CreateComparisonResultsModel(IEnumerable<string> schemes, IContent content)
        {
            return new ComparisonResultsModel(
                content.Schemes.Where(x => schemes.Contains(x.HtmlId)));
        }

        public ComparisonResultsModel CreateComparisonResultsModel(IEnumerable<string> schemes)
        {
            return CreateComparisonResultsModel(schemes, _contentService.Content);
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

        public async Task<ComparisonResultsModel> CreateComparisonResultsModelPreview(IEnumerable<string> schemes)
        {
            IContent previewContent = await _contentService.UpdatePreview();

            var comparisonResultsModel = CreateComparisonResultsModel(schemes, previewContent);
            comparisonResultsModel.Preview = new PreviewModel(Enumerable.Empty<HtmlString>());

            return comparisonResultsModel;
        }

        private ReadOnlyDictionary<string, SchemeDetailsModel> BuildSchemeDetailsModelsDictionary()
        {
            var schemeDetailsModels = new Dictionary<string, SchemeDetailsModel>();

            foreach (string schemeUrl in _contentService.Content.Schemes.Select(s => s.Url))
            {
                schemeDetailsModels.Add(schemeUrl, new SchemeDetailsModel(schemeUrl, _contentService.Content.Schemes));
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
                var model = new SchemeDetailsModel(schemeUrl, previewContent.Schemes);
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

            if (model.Preamble == null)
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

            if (model.Scheme.Description == null)
            {
                errors.Add(new HtmlString("If there is no details page override, the description must not be blank"));
            }

            if (model.Scheme.SubSchemes.Any())
                return errors;

            if (model.Scheme.Cost == null)
            {
                errors.Add(new HtmlString("If there is no details page override and no sub schemes, the cost must not be blank"));
            }
            if (model.Scheme.Responsibility == null)
            {
                errors.Add(new HtmlString("If there is no details page override and no sub schemes, the responsibility must not be blank"));
            }
            if (model.Scheme.Benefits == null)
            {
                errors.Add(new HtmlString("If there is no details page override and no sub schemes, the benefits must not be blank"));
            }
            if (model.Scheme.OfferHeader == null)
            {
                errors.Add(new HtmlString("If there is no details page override and no sub schemes, the offer header must not be blank"));
            }
            if (model.Scheme.Offer == null)
            {
                errors.Add(new HtmlString("If there is no details page override and no sub schemes, the offer must not be blank"));
            }

            return errors;
        }
    }
}
