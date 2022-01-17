using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class HomeModel
    {
        public IHtmlContent Preamble { get; }
        public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<FilterGroupModel> FilterGroupModels { get; }
        public bool EnsureSchemesAreVisible { get; }

        private readonly IContent _content;

        public HomeModel(
            IHtmlContent preamble,
            IEnumerable<Scheme> schemes,
            IEnumerable<FilterGroupModel> filterGroupModels,
            IContent content,
            bool ensureSchemesAreVisible = false)
        {
            Preamble = preamble;
            Schemes = schemes;
            FilterGroupModels = filterGroupModels;
            _content = content;
            EnsureSchemesAreVisible = ensureSchemesAreVisible;
        }

        //todo: remove this and use events instead - lockless!
        public bool IsBasedOn(IContent content)
        {
            return _content == content;
        }
    }
}