using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class SchemeDetailsModel
    {
        public SchemeDetailsModel(string schemeUrl, IEnumerable<Scheme> schemes)
        {
            Scheme = schemes.First(s => s.Url == schemeUrl);

            OtherSchemes = schemes.Where(s => s.Url != schemeUrl);
        }

        public Scheme Scheme { get; }
        public IEnumerable<Scheme> OtherSchemes { get; }
    }
}
