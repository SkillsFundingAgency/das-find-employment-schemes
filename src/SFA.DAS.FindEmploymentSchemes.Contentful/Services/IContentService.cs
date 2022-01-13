using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public interface IContentService
    {
        Task<IContent> Get();
    }
}
