using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{

    public class ContactModel : LayoutModel
    {

        public List<Contact> ContactList { get; set; } = new List<Contact>();

    }

}
