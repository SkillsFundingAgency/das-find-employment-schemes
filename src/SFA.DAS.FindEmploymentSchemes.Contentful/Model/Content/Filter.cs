using System.Collections.Generic;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    public class Filter //: IFilter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        ///// <summary>
        ///// Guaranteed in ascending Order order
        ///// </summary>
        public IEnumerable<IFilterAspect> Aspects { get; set; }

        public Filter(string name, string description, IEnumerable<IFilterAspect> aspects)
        {
            Name = name;
            Description = description;
            Aspects = aspects;
        }
    }
}