using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class LayoutModel
    {
        public PreviewModel Preview { get; set; }

        public LayoutModel()
        {
            Preview = PreviewModel.NotPreviewModel;
        }
    }
}
