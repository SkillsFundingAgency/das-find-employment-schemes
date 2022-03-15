using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class HomeModel
    {
        public IHtmlContent? Preamble { get; }
        public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<Filter> Filters { get; }
        public bool EnsureSchemesAreVisible { get; }
        public PreviewModel Preview { get; set; }

        public HomeModel(
            IHtmlContent? preamble,
            IEnumerable<Scheme> schemes,
            IEnumerable<Filter> filters,
            bool ensureSchemesAreVisible = false)
        {
            Preamble = preamble;
            Schemes = schemes;
            Filters = filters;
            EnsureSchemesAreVisible = ensureSchemesAreVisible;
            Preview = PreviewModel.NotPreviewModel;
        }
    }
}