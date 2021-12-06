using System.Diagnostics;
using Contentful.Core.Models;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.ContentCodeGenerator.Model
{
    [DebuggerDisplay("{Title}")]
    public class Page
    {
        public string? Title { get; set; }
        public string? Url { get; set; }
        public Document? Content { get; set; }
    }
}
