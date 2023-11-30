using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    public interface ISchemesModelService
    {
        HomeModel HomeModel { get; set; }
        ComparisonModel ComparisonModel { get; }
        ComparisonResultsModel CreateComparisonResultsModel(IEnumerable<string> schemes, SchemeFilterModel filters);
        Task<HomeModel> CreateHomeModelPreview();
        Task<ComparisonModel> CreateComparisonModelPreview();
        Task<ComparisonResultsModel> CreateComparisonResultsModelPreview(IEnumerable<string> schemes, SchemeFilterModel filters);
        SchemeDetailsModel? GetSchemeDetailsModel(string schemeUrl);
        Task<SchemeDetailsModel?> GetSchemeDetailsModelPreview(string schemeUrl);
    }
}
