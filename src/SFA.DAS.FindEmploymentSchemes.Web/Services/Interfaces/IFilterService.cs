using SFA.DAS.FindEmploymentSchemes.Web.Models;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{

    public interface IFilterService
    {

        HomeModel ApplyFilter(SchemeFilterModel filters);

        SchemeFilterModel CreateFilterModel(string pay, string duration, string motivation);

        HomeModel RemapFilters(string pay, string duration, string motivation);

    }

}
