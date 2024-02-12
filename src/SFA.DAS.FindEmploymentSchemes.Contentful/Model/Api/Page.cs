using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api
{

    [DebuggerDisplay("{Title}")]
    [ExcludeFromCodeCoverage]
    public class Page : IRootContent
    {

        public string? Title { get; set; }

        public string? Url { get; set; }

        public InterimBreadcrumbs? InterimBreadcrumbs { get; set; }

        public InterimPreamble? InterimPreamble { get; set; }

        public Document? Content { get; set; }

    }

}
