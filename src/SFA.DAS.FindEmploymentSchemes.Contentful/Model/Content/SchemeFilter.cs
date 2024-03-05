using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{

    public class SchemeFilter
    {

        public required string SchemeFilterName { get; set; }

        public required string SchemeFilterPrefix { get; set; }

        public required string SchemeFilterDescription { get; set; }

        public required int SchemeFilterOrder { get; set; }

        public List<SchemeFilterAspect> SchemeFilterAspects { get; set; } = [];

    }

}
