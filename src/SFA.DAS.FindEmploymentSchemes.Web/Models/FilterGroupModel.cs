using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class FilterGroupModel
    {
        public string Description { get; set; }

        public IEnumerable<IFilter> Filters { get; set; }

        public FilterGroupModel(string description, IEnumerable<IFilter> filters)
        {
            Description = description;
            Filters = filters;
        }
    }
}
