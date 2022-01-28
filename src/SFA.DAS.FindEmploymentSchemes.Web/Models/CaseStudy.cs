using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class CaseStudy
    {
        public string Name { get; set; }
        public string DisplayTitle { get; set; }
        public IHtmlContent Content { get; set; }

        public CaseStudy(string name, string title, IHtmlContent content)
        {
            DisplayTitle = title;
            Name = name;
            Content = content;
        }
    }
}
