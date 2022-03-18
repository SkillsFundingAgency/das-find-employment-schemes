using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    [ExcludeFromCodeCoverage]
    public class MotivationFilterService : FilterService, IMotivationFilterService
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
