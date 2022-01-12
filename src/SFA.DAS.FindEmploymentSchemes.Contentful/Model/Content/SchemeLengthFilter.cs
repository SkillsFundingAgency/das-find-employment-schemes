using System.Diagnostics;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [DebuggerDisplay("{Id}")]
    public class SchemeLengthFilter : IFilter
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }

        public SchemeLengthFilter(string id, string description, bool selected = false)
        {
            Id = id;
            Description = description;
            Selected = selected;
        }
    }
}
