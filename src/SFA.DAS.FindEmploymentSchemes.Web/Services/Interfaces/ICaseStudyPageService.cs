using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    //todo: rename model services/orchestrators consistently
    public interface ICaseStudyPageService
    {
        CaseStudyPageModel? GetCaseStudyPageModel(string pageUrl);
        Task<CaseStudyPageModel?> GetCaseStudyPageModelPreview(string pageUrl);
    }
}
