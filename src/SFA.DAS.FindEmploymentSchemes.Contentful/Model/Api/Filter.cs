using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api
{
    [DebuggerDisplay("{Name}")]
    [ExcludeFromCodeCoverage]
    public class Filter : IFilter
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
    }
}