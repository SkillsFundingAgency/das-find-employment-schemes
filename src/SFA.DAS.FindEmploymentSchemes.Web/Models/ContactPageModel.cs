using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{

    public class ContactPageModel : LayoutModel
    {

        public required string ContactPageTitle { get; set; }

        public InterimPreamble? InterimPreamble { get; set; }

        public InterimBreadcrumbs? InterimBreadcrumbs { get; set; }

        public List<Contact> Contacts { get; set; } = [];

    }

}
