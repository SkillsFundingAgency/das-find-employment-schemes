using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces
{
    //todo: rename these and others to ..Orchestrator (e.g. ICaseStudiesOrchestrator), to distinguish between orchs and modelsservices?
    // or combine orchestrators and modelservices?
    public interface ICaseStudyPageService
    {
        (string?, CaseStudyPage?) CaseStudyPage(string pageUrl, IContent content);
    }
}
