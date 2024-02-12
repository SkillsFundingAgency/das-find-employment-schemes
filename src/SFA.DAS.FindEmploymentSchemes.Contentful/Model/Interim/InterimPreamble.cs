using Contentful.Core.Models;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    public class InterimPreamble
    {

        public string? PreambleTitle { get; set; }

        public Document? PrimarySection { get; set; }

        public Document? SecondarySection { get; set; }

        public required bool InterimPreambleHideNavigation { get; set; }

    }

}
