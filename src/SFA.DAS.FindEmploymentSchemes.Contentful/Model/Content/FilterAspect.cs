using System.Diagnostics;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [DebuggerDisplay("{Id}")]
    public class FilterAspect : IFilterAspect
    {
        public string Id { get; }
        public string Description { get; }
        public bool Selected { get; }

        public FilterAspect(string id, string description, bool selected = false)
        {
            Id = id;
            Description = description;
            Selected = selected;
        }
    }
}
