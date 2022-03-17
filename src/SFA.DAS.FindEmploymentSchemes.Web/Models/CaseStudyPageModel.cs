using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class CaseStudyPageModel : LayoutModel
    {
        public CaseStudyPage CaseStudyPage { get; }

        public CaseStudyPageModel(CaseStudyPage caseStudyPage)
        {
            CaseStudyPage = caseStudyPage;
        }
    }
}
