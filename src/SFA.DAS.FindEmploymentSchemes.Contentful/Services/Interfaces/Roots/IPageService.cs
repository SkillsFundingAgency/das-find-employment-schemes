using System.Collections.Generic;
using System.Threading.Tasks;
using Contentful.Core;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots
{
    public interface IPageService
    {
        Task<IEnumerable<Page>> GetAll(IContentfulClient contentfulClient);
    }
}
