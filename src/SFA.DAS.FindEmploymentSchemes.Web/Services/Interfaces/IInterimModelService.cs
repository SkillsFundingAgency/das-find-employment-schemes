using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{

    public interface IInterimModelService
    {

        LandingModel? GetLandingModel();

        Task<LandingModel?> GetLandingPreviewModel();

        InterimPageModel? GetInterimPageModel(string interimURL);

        Task<InterimPageModel?> GetInterimPagePreviewModel(string interimURL);

    }

}
