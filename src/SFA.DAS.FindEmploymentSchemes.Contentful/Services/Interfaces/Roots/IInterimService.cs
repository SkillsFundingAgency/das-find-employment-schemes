using Contentful.Core;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots
{

    public interface IInterimService
    {

        Task<InterimLandingPage?> GetLandingPage(IContentfulClient contentfulClient);

        Task<IEnumerable<InterimMenuItem>> GetMenuItems(IContentfulClient contentfulClient);

        Task<IEnumerable<InterimPage>> GetInterimPages(IContentfulClient contentfulClient);

    }

}
