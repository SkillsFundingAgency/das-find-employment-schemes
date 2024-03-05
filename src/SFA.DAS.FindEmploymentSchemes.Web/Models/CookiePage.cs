using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    [ExcludeFromCodeCoverage]
    public class CookiePage : Page
    {

        public bool ShowMessage { get; }

        public Page AnalyticsPage { get; }

        public Page MarketingPage { get; }

        //todo: common const cookies url
        //todo: page interface??
        public CookiePage(
            
            Page analyticsPage, 
            
            Page marketingPage, 
            
            bool showMessage, 
            
            InterimBreadcrumbs? interimBreadcrumbs = null,

            InterimPreamble? interimPreamble = null

        )
            : base(analyticsPage.Title, "cookies", analyticsPage.Content, interimPreamble, interimBreadcrumbs)
        {

            ShowMessage = showMessage;

            AnalyticsPage = analyticsPage;

            MarketingPage = marketingPage;

        }

    }

}
