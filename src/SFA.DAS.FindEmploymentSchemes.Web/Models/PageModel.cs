using System.Diagnostics.CodeAnalysis;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class PageModel : LayoutModel
    {
        public Page Page { get; }
        public string? ViewName { get; }

        public PageModel(Page page, string? viewName = null)
        {
            Page = page;
            ViewName = viewName;
        }
    }
}
