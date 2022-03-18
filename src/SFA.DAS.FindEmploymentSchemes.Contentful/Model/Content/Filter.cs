using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
    public class Filter
    {
        public string Name { get; }
        public string Description { get; }
        /// <summary>
        /// Guaranteed in ascending Order order
        /// </summary>
        public IEnumerable<IFilterAspect> Aspects { get; }

        public Filter(string name, string description, IEnumerable<IFilterAspect> aspects)
        {
            Name = name;
            Description = description;
            Aspects = aspects;
        }
    }
}