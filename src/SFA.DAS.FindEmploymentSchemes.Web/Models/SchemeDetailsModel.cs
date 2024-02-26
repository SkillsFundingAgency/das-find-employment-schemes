using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class SchemeDetailsModel : LayoutModel
    {
        public Scheme Scheme { get; }
        public IEnumerable<Scheme> Schemes { get; }

        public SchemeDetailsModel(string schemeUrl, IEnumerable<Scheme> schemes, IEnumerable<InterimMenuItem> menuItems, BetaBanner? banner, InterimFooterLinks? footerLinks)
        {
            Schemes = schemes.ToArray();
            Scheme = Schemes.First(s => s.Url == schemeUrl);
            MenuItems = menuItems;
            BetaBanner = banner;
            InterimFooterLinks = footerLinks;
        }
    }
}
