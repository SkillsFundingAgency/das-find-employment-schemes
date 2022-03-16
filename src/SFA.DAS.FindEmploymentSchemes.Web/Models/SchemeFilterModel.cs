using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class SchemeFilterModel
    {
        public string[] Motivations { get; set; }
        public string[] SchemeLength { get; set; }
        public string[] Pay { get; set; }

        public IEnumerable<string> AllFilters => Motivations.Union(SchemeLength).Union(Pay);

        public SchemeFilterModel()
        {
            Motivations = Array.Empty<string>();
            SchemeLength = Array.Empty<string>();
            Pay = Array.Empty<string>();
        }

        public SchemeFilterModel(string[] motivations, string[] schemeLength, string[] pay)
        {
            Motivations = motivations;
            SchemeLength = schemeLength;
            Pay = pay;
        }
    }
}
