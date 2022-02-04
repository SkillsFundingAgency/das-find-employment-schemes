using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public interface IPageService
    {
        (string?, Page?) Page(string pageUrl, IContent content);
    }
}
