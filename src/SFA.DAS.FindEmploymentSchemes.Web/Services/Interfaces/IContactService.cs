using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{

    public interface IContactService
    {

        ContactPageModel? GetContactPageModel();

        Task<ContactPageModel?> GetContactPreviewPageModel();

    }

}
