using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Interfaces
{

    public interface IViewRenderService
    {

        Task<string> RenderToStringAsync<TModel>(string viewName, TModel model);

    }

}
