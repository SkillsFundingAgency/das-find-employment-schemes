using System.Diagnostics;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [DebuggerDisplay("{Id}")]
    public class FilterAspect : IFilterAspect
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }

        //todo: yukk - need factory/builder to create immutable filter using generics
        public FilterAspect()
        {
            Id = null!;
            Description = null!;
        }

        public FilterAspect(string id, string description, bool selected = false)
        {
            Id = id;
            Description = description;
            Selected = selected;
        }
    }
}
