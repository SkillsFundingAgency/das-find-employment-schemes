using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    public class MotivationFilterService : FilterService
    {
        private const string Name = "motivations";
        private const string Description = "I want to";
        private const string ContentfulTypeName = "motivationsFilter";
        public static string Prefix => "motivations";

        public MotivationFilterService(HtmlRenderer htmlRenderer)
            : base(htmlRenderer, Name, Description, ContentfulTypeName, Prefix)
        {
        }
    }
}
