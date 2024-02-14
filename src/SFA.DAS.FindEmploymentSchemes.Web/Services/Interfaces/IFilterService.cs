using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{

    public interface IFilterService
    {

        HomeModel ApplyFilter(SchemeFilterModel filters);

        SchemeFilterModel CreateFilterModel(string filters);

        HomeModel RemapFilters(string filters);

    }

}
