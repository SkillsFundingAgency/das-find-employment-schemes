using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    //todo: base PreviewableModel?
    public class CaseStudyPageModel
    {
        public CaseStudyPage CaseStudyPage { get; }
        public PreviewModel Preview { get; set; }

        public CaseStudyPageModel(CaseStudyPage caseStudyPage)
        {
            CaseStudyPage = caseStudyPage;
            Preview = PreviewModel.NotPreviewModel;
        }
    }
}
