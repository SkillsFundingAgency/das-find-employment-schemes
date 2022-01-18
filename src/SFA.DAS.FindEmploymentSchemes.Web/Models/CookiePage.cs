
namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class CookiePage : Page
    {
        public bool ShowMessage { get; set; }
        public Page AnalyticsPage { get; set; }
        public Page MarketingPage { get; set; }
        public CookiePage(Page analyticsPage, Page marketingPage, bool showMessage) : base(analyticsPage.Title, "cookies", analyticsPage.Content)
        {
            this.ShowMessage = showMessage;
            this.AnalyticsPage = analyticsPage;
            this.MarketingPage = marketingPage;
        }
    }
}
