using System.Collections.Generic;
using System.Threading.Tasks;
using Contentful.Core;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots
{
    public interface ISchemeService
    {
        Task<IEnumerable<Scheme>> GetAll(IContentfulClient contentfulClient);
    }
}
