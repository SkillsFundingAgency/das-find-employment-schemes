using System.Collections.Generic;
using System.Threading.Tasks;
using Contentful.Core;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots
{
    public interface ICaseStudyPageService
    {
        Task<IEnumerable<CaseStudyPage>> GetAll(IContentfulClient contentfulClient, IEnumerable<Scheme> schemes);
    }
}
