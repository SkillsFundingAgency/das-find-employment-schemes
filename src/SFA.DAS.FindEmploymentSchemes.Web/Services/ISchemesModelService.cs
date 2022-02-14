using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public interface ISchemesModelService
    {
        HomeModel HomeModel { get; }
        SchemeDetailsModel? GetSchemeDetailsModel(string schemeUrl);
    }
}
