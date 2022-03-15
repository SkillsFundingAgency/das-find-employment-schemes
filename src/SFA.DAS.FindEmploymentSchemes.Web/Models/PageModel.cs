using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class PageModel
    {
        public Page Page { get; }
        public string? ViewName { get; }
        public PreviewModel Preview { get; set; }

        public PageModel(Page page, string? viewName = null)
        {
            Page = page;
            ViewName = viewName;
            Preview = PreviewModel.NotPreviewModel;
        }
    }
}
