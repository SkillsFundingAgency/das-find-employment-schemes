using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    public interface ISchemesModelService
    {
        HomeModel HomeModel { get; }
        Task<HomeModel> CreateHomeModelPreview();
        SchemeDetailsModel? GetSchemeDetailsModel(string schemeUrl);
        Task<SchemeDetailsModel?> GetSchemeDetailsModelPreview(string schemeUrl);
    }
}
