using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    public interface IPageService
    {
        (string?, Page?) Page(string pageUrl, IContent content);
        (string? routeName, object? routeValues) RedirectPreview(string pageUrl);
    }
}
