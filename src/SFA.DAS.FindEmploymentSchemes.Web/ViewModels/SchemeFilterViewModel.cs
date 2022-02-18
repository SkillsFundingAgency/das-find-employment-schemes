using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindEmploymentSchemes.Web.ViewModels
{
    public class SchemeFilterViewModel
    {
        public string[] Motivations { get; set; }
        public string[] SchemeLength { get; set; }
        public string[] Pay { get; set; }
        public IEnumerable<string> AllFilters => Motivations.Union(SchemeLength).Union(Pay);

        public SchemeFilterViewModel(string[] motivations, string[] schemeLength, string[] pay)
        {
            Motivations = motivations;
            SchemeLength = schemeLength;
            Pay = pay;
        }
    }
}
