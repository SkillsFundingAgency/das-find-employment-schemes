using Contentful.Core.Models;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces.Roots;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots.Base;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services.Roots
{
    [ExcludeFromCodeCoverage]
    public class PayFilterService : FilterService, IPayFilterService
    {
        private const string Name = "pay";
        private const string Description = "I can offer";
        private const string ContentfulTypeName = "payFilter";
        public static string Prefix => "pay";

        public PayFilterService(HtmlRenderer htmlRenderer)
            : base(htmlRenderer, Name, Description, ContentfulTypeName, Prefix)
        {
        }
    }
}
