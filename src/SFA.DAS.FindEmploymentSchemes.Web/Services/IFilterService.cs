
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.ViewModels;


namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public interface IFilterService
    {
        FilterGroupModel[] FilterGroupModels();
        HomeModel HomeModel();
        IReadOnlyDictionary<string, SchemeDetailsModel> SchemeDetailsModels();
        ReadOnlyDictionary<string, SchemeDetailsModel> BuildSchemeDetailsModelsDictionary();
        HomeModel ApplyFilter(SchemeFilterViewModel filters);
    }
}
