using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    //todo: just use Filter instead?
    public class FilterGroupModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<IFilterAspect> Filters { get; set; }

        public FilterGroupModel(string name, string description, IEnumerable<IFilterAspect> filters)
        {
            Name = name;
            Description = description;
            Filters = filters;
        }
    }
}
