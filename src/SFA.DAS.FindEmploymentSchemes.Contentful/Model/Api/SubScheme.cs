using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Contentful.Core.Models;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api
{
    [DebuggerDisplay("{Title}")]
    [ExcludeFromCodeCoverage]
    public class SubScheme
    {
        public string? Title { get; set; }
        public Document? Summary { get; set; }
        public Document? Content { get; set; }
    }
}
