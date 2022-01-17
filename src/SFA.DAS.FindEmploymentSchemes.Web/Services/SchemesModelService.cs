using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services;
using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public interface ISchemesModelService
    {
        HomeModel HomeModel { get; }
        SchemeDetailsModel? GetSchemeDetailsModel(string schemeUrl);
    }

    //todo: singleton
    public class SchemesModelService : ISchemesModelService
    {
        private const string HomepagePreambleUrl = "home";

        //todo: we could move these into IContent (filter contain name, description & filters)
        private const string MotivationName = "motivations";
        private const string MotivationDescription = "I want to";
        private const string SchemeLengthName = "schemeLength";
        private const string SchemeLengthDescription = "Length of scheme?";
        private const string PayName = "pay";
        private const string PayDescription = "I can offer";

        private readonly IContentService _contentService;
        private HomeModel _homeModel;

        public SchemesModelService(IContentService contentService)
        {
            _contentService = contentService;
            _homeModel = CreateHomeModel(_contentService.Content);
            _schemeDetailsModels = BuildSchemeDetailsModelsDictionary();
            _schemeDetailsModelsContent = _contentService.Content;
        }

        public HomeModel HomeModel
        {
            //todo: locking : need to be careful - don't want to keep creating homemodel, but also don't want to bottleneck with locking
            // can we use a no-locking solution?
            // might be best to create the model each time and not lock?
            get
            {
                if (!_homeModel.IsBasedOn(_contentService.Content))
                {
                    _homeModel = CreateHomeModel(_contentService.Content);
                }

                return _homeModel;
            }
        }

        private HomeModel CreateHomeModel(IContent content)
        {
            return new HomeModel(
                content.Pages.First(p => p.Url == HomepagePreambleUrl).Content,
                content.Schemes,
                new[] {
                    new FilterGroupModel(MotivationName, MotivationDescription, content.MotivationsFilters),
                    new FilterGroupModel(SchemeLengthName, SchemeLengthDescription, content.SchemeLengthFilters),
                    new FilterGroupModel(PayName, PayDescription, content.PayFilters)
                },
                content);
        }

        //todo: locking / create model each time??
        private IContent _schemeDetailsModelsContent;
        private IReadOnlyDictionary<string, SchemeDetailsModel> _schemeDetailsModels;
        private IReadOnlyDictionary<string, SchemeDetailsModel> SchemeDetailsModels
        {
            get
            {
                //if (!_homeModel.IsBasedOn(_contentService.Content))
                if (_schemeDetailsModelsContent != _contentService.Content)
                {
                    _schemeDetailsModels = BuildSchemeDetailsModelsDictionary();
                    _schemeDetailsModelsContent = _contentService.Content;
                }

                return _schemeDetailsModels;
            }
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
            SchemeDetailsModels.TryGetValue(schemeUrl, out SchemeDetailsModel? schemeDetailsModel);
            return schemeDetailsModel;
        }
    }
}
