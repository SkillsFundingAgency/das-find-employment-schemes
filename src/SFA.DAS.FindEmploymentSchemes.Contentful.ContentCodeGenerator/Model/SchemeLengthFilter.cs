using System.Diagnostics;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.ContentCodeGenerator.Model
{
    [DebuggerDisplay("{Name}")]
    public class SchemeLengthFilter : IFilter
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
    }
}
