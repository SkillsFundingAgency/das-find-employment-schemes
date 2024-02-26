using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    public class InterimFooterLinks
    {

        public required string InterimFooterLinksID { get; set; }

        public required string InterimFooterLinksTitle { get; set; }

        public string? InterimFooterLinksPrimarySectionTitle { get; set; }

        public string? InterimFooterLinksSecondarySectionTitle { get; set; }

        public List<InterimFooterLink> InterimFooterLinksPrimarySectionLinks { get; set; } = [];

        public List<InterimFooterLink> InterimFooterLinksSecondarySectionLinks { get; set; } = [];

    }

}
