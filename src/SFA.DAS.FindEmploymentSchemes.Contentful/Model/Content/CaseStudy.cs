using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    [ExcludeFromCodeCoverage]
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