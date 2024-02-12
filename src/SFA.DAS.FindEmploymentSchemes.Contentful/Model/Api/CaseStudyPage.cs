using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api
{
    [DebuggerDisplay("{Title}")]
    [ExcludeFromCodeCoverage]
    public class CaseStudyPage : IRootContent
    {
        public string? Title { get; set; }
        public string? Url { get; set; }
        public Document? Content { get; set; }
        public Scheme? Scheme { get; set; }
    }
}
