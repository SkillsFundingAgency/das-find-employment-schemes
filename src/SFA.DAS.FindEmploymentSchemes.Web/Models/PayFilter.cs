using System.Diagnostics;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [DebuggerDisplay("{Id}")]
    public class PayFilter : IFilter
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public PayFilter(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}
