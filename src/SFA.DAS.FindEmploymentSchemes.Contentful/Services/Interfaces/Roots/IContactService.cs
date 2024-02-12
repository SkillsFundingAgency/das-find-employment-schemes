using Contentful.Core;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots
{

    public interface IContactService
    {

        Task<ContactPage?> GetContactPage(IContentfulClient contentfulClient);

    }

}
