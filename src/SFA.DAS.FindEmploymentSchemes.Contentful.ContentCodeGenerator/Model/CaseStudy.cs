
using System.Collections.Generic;
using System.Diagnostics;
using Contentful.Core.Models;


namespace SFA.DAS.FindEmploymentSchemes.Contentful.ContentCodeGenerator.Model
{
    [DebuggerDisplay("{Name}")]
    public class CaseStudy
    {
        public string? Name { get; set; }
        public string? DisplayTitle { get; set; }
        public Document? Description { get; set; }
    }
}
