using Contentful.Core.Models;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim
{

    [ExcludeFromCodeCoverage]
    public class InterimPreamble
    {

        public string? PreambleTitle { get; set; }

        public Document? PrimarySection { get; set; }

        public Document? SecondarySection { get; set; }

    }

}
