using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    public interface IPageService
    {
        PageModel? GetPageModel(string pageUrl);
        PageModel? GetCookiePageModel(IContent content, bool showMessage);
        Task<PageModel?> GetPageModelPreview(string pageUrl);
        (string? routeName, object? routeValues) RedirectPreview(string pageUrl);
    }
}
