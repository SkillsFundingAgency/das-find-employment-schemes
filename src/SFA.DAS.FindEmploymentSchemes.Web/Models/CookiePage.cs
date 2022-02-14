using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class CookiePage : Page
    {
        public bool ShowMessage { get; }
        public Page AnalyticsPage { get; }
        public Page MarketingPage { get; }

        public CookiePage(Page analyticsPage, Page marketingPage, bool showMessage) : base(analyticsPage.Title, "cookies", analyticsPage.Content)
        {
            ShowMessage = showMessage;
            AnalyticsPage = analyticsPage;
            MarketingPage = marketingPage;
        }
    }
}
