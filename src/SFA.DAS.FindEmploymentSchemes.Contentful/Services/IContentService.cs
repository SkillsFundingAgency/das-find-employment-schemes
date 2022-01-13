using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public interface IContentService
    {
        Task<IContent> Get();
    }
}
