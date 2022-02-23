using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class SchemeDetailsModel
    {
        public SchemeDetailsModel(string schemeUrl, IEnumerable<Scheme> schemes)
        {
            Scheme = schemes.First(s => s.Url == schemeUrl);
            Schemes = schemes;
        }

        public Scheme Scheme { get; }
        public IEnumerable<Scheme> Schemes { get; }
    }
}
