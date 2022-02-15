using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    public class CaseStudy
    {
        public string Name { get; }
        public string DisplayTitle { get; }
        public HtmlString Content { get; }

        public CaseStudy(string name, string title, HtmlString content)
        {
            DisplayTitle = title;
            Name = name;
            Content = content;
        }
    }
}