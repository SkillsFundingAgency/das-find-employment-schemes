using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    public interface ISchemesModelService
    {
        HomeModel HomeModel { get; }
        SchemeDetailsModel? GetSchemeDetailsModel(string schemeUrl);

        HomeModel CreateHomeModel(IContent content);
    }
}
