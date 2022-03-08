using System.Diagnostics.CodeAnalysis;
using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    [ExcludeFromCodeCoverage]
    public class SchemeLengthFilterService : FilterService, ISchemeLengthFilterService
    {
        private const string Name = "schemeLength";
        private const string Description = "Length of scheme?";
        private const string ContentfulTypeName = "schemeLengthFilter";
        public static string Prefix => "scheme-length";

        public SchemeLengthFilterService(HtmlRenderer htmlRenderer)
            : base(htmlRenderer, Name, Description, ContentfulTypeName, Prefix)
        {
        }
    }
}
