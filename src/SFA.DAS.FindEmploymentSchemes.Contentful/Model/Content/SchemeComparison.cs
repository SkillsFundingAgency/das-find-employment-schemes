using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{

    [ExcludeFromCodeCoverage]
    public class SchemeComparison
    {

        public required string SchemeComparisonTitle { get; set; }

        public required string SchemeComparisonActionButtonText { get; set; }

        public required string SchemeComparisonTitleColumnHeading { get; set; }

        public required string SchemeComparisonRecruitOrTrainHeading { get; set; }

        public required string SchemeComparisonAgeHeading { get; set; }

        public required string SchemeComparisonCostHeading { get; set; }

        public required string SchemeComparisonDurationHeading { get; set; }

        public InterimBreadcrumbs? InterimBreadcrumbs { get; set; }

        public InterimPreamble? InterimPreamble { get; set; }

    }

}
