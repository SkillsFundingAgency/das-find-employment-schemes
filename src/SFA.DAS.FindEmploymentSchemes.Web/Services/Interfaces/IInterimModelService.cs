using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{

    public interface IInterimModelService
    {

        InterimPageModel? GetInterimPageModel(string interimURL);

        LandingModel? GetLandingModel();

    }

}
