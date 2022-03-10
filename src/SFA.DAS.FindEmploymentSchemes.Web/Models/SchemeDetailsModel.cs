using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class SchemeDetailsModel
    {
        public SchemeDetailsModel(string schemeUrl, IEnumerable<Scheme> schemes)
        {
            Schemes = schemes.ToArray();
            Scheme = Schemes.First(s => s.Url == schemeUrl);
            Preview = PreviewModel.NotPreviewModel;
        }

        public Scheme Scheme { get; }
        public IEnumerable<Scheme> Schemes { get; }
        public PreviewModel Preview { get; set; }
    }
}
