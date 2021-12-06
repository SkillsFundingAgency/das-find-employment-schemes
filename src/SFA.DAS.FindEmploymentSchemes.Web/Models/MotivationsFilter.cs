using System.Diagnostics;
using SFA.DAS.FindEmploymentSchemes.Web.Models.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [DebuggerDisplay("{Id}")]
    public class MotivationsFilter : IFilter
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }

        public MotivationsFilter(string id, string description, bool selected = false)
        {
            Id = id;
            Description = description;
            Selected = selected;
        }
    }
}
