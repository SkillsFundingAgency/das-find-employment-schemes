
using System.Collections.Generic;
using System.Linq;


namespace SFA.DAS.FindEmploymentSchemes.Web.ViewModels
{

    public class SchemeFilterViewModel
    {
        public string[] motivations { get; set; }
        public string[] schemeLength { get; set; }
        public string[] pay { get; set; }
        public IEnumerable<string> allFilters=> motivations.Union(schemeLength).Union(pay);

        public SchemeFilterViewModel()
        {
            motivations = new string[] { };
            schemeLength = new string[] { };
            pay = new string[] { };
        }

        public SchemeFilterViewModel(string[] motivations, string[] schemeLength, string[] pay)
        {
            this.motivations = motivations;
            this.schemeLength = schemeLength;
            this.pay = pay;
        }
    }
}
