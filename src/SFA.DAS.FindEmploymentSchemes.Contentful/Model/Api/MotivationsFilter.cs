using System.Diagnostics;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Api
{
    [DebuggerDisplay("{Name}")]
    public class MotivationsFilter : IFilter
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
    }
}
