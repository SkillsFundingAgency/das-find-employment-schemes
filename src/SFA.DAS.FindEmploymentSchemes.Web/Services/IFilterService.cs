using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public interface IFilterService
    {
        //HomeModel HomeModel { get; set; }
        //IReadOnlyDictionary<string, SchemeDetailsModel> SchemeDetailsModels { get; }
        HomeModel ApplyFilter(SchemeFilterViewModel filters);
    }
}
