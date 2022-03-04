using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using Contentful.Core.Models;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    public class PageService : ContentRootService
    {
        public PageService(HtmlRenderer htmlRenderer) : base(htmlRenderer)
        {
        }
    }
}
