using System.Collections.Generic;
using Microsoft.AspNetCore.Html;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{
    public class PageModelService : IPageModelService
    {
        public IEnumerable<HtmlString> GetErrors(Page page)
        {
            var errors = new List<HtmlString>();
            if (string.IsNullOrWhiteSpace(page.Title))
            {
                errors.Add(new HtmlString("Title must not be blank"));
            }

            if (page.Content == null)
            {
                errors.Add(new HtmlString("Content must not be blank"));
            }

            return errors;
        }
    }
}
