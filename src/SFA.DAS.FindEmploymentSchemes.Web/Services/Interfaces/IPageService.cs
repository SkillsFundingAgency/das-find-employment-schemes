using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    public interface IPageService
    {
        PageModel? GetPageModel(string pageUrl);
        Task<PageModel?> GetPageModelPreview(string pageUrl);
        (string? routeName, object? routeValues) RedirectPreview(string pageUrl);
    }
}
