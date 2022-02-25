
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
            Errors = Enumerable.Empty<PreviewContentError>();
        }
        public SchemeDetailsModel(string schemeUrl, IEnumerable<Scheme> schemes, IEnumerable<PreviewContentError> errors)
        {
            Scheme = schemes.First(s => s.Url == schemeUrl);
            Schemes = schemes;
            Errors = errors;
        }

        public Scheme Scheme { get; }
        public IEnumerable<Scheme> Schemes { get; }
        public IEnumerable<PreviewContentError> Errors { get; }
    }
}
