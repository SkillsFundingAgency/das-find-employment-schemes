using System.Diagnostics;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.ContentCodeGenerator.Model
{
    [DebuggerDisplay("{Name}")]
    public class MotivationsFilter : IFilter
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
