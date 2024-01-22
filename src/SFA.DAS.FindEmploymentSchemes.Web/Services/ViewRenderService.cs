using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindEmploymentSchemes.Web.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{

    public class ViewRenderService : IViewRenderService
    {

        private readonly ILogger<ViewRenderService> _logger;

        private readonly IServiceProvider _serviceProvider;

        private readonly ICompositeViewEngine _viewEngine;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ViewRenderService(
            
            ILogger<ViewRenderService> logger,

            IServiceProvider serviceProvider,

            ICompositeViewEngine viewEngine,

            IHttpContextAccessor httpContextAccessor

        ) 
        {

            _logger = logger;

            _serviceProvider = serviceProvider;

            _viewEngine = viewEngine;

            _httpContextAccessor = httpContextAccessor;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> RenderToStringAsync<TModel>(string viewName, TModel model)
        {

            try
            {

                var httpContext = _httpContextAccessor.HttpContext;

                #pragma warning disable CS8604 // Possible null reference argument.

                var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

                #pragma warning restore CS8604 // Possible null reference argument.

                using (var sw = new StringWriter())
                {

                    var viewResult = _viewEngine.FindView(actionContext, viewName, false);

                    if (viewResult.View == null)
                    {
                        throw new ArgumentNullException($"{viewName} does not match any available view");
                    }

                    var viewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {

                        Model = model

                    };

                    var viewContext = new ViewContext(

                        actionContext,

                        viewResult.View,

                        viewData,

                        new TempDataDictionary(actionContext.HttpContext, _serviceProvider.GetRequiredService<ITempDataProvider>()),

                        sw,

                        new HtmlHelperOptions()

                    );

                    await viewResult.View.RenderAsync(viewContext);

                    return sw.ToString();

                }

            }
            catch(Exception _exception)
            {

                _logger.LogError(_exception, "Unable to render partial view {PartialViewName}", viewName);

                return string.Empty;

            }

        }

    }

}
