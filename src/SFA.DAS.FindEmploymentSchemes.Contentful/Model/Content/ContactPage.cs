using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{

    [ExcludeFromCodeCoverage]
    public class ContactPage
    {

        public required string ContactPageTitle { get; set; }

        public InterimPreamble? InterimPreamble { get; set; }

        public InterimBreadcrumbs? InterimBreadcrumbs { get; set; }

        public List<Contact> Contacts { get; set; } = [];

    }

}
