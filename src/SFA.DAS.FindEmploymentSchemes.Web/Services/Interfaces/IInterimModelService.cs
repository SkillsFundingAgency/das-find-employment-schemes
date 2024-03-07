using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{

    public interface IInterimModelService
    {

        InterimPageModel? GetInterimPageModel(string interimURL);

        LandingModel? GetLandingModel();

        Task<LandingModel?> GetLandingPreviewModel();

    }

}
