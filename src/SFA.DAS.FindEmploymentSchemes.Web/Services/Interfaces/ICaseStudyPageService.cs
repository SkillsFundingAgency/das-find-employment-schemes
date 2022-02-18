using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    public interface ICaseStudyPageService
    {
        (string?, CaseStudyPage?) CaseStudyPage(string pageUrl, IContent content);
    }
}
