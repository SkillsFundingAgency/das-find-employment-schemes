using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    public interface ISchemesModelService
    {
        HomeModel HomeModel { get; }
        ComparisonModel ComparisonModel { get; }
        ComparisonResultsModel CreateComparisonResultsModel(string[] schemes);
        Task<HomeModel> CreateHomeModelPreview();
        Task<ComparisonModel> CreateComparisonModelPreview();
        Task<ComparisonResultsModel> CreateComparisonResultsModelPreview(string[] schemes);
        SchemeDetailsModel? GetSchemeDetailsModel(string schemeUrl);
        Task<SchemeDetailsModel?> GetSchemeDetailsModelPreview(string schemeUrl);
    }
}
