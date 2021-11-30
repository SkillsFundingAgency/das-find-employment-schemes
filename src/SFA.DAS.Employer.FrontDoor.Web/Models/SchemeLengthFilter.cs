using System.Diagnostics;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [DebuggerDisplay("{Id}")]
    public class SchemeLengthFilter : IFilter
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public SchemeLengthFilter(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}
