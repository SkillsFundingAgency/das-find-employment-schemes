using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class FilterGroupModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<IFilter> Filters { get; set; }

        public FilterGroupModel(string name, string description, IEnumerable<IFilter> filters)
        {
            Name = name;
            Description = description;
            Filters = filters;
        }
    }
}
