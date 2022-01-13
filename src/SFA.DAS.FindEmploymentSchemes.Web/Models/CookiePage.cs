
namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class CookiePage : Page
    {
        public bool ShowMessage { get; set; }
        public CookiePage(Page page, bool showMessage) : base(page.Title, page.Url, page.Content)
        {
            this.ShowMessage = showMessage;
        }
    }
}
