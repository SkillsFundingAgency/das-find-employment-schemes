using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{

    public interface IFilterService
    {

        Task<HomeModel> ApplyFilter(SchemeFilterModel filters, bool isPreview = false);

        SchemeFilterModel CreateFilterModel(string filters);

        Task<HomeModel> RemapFilters(string filters, bool isPreview = false);

    }

}
