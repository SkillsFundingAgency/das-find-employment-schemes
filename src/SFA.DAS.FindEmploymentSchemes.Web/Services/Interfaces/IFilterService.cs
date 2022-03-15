using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    public interface IFilterService
    {
        HomeModel ApplyFilter(SchemeFilterModel filters);
    }
}
