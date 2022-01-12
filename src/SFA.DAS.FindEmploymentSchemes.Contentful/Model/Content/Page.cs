using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    //todo: interface?
    public class Page
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public IHtmlContent Content { get; set; }

        public Page(string title, string url, IHtmlContent content)
        {
            Title = title;
            Url = url;
            Content = content;
        }

        //public Page(Api.Page apiPage)
        //{
        //    //todo: can any of these come through as null?
        //    Title = apiPage.Title!;
        //    Url = apiPage.Url!;
        //    Content = apiPage.Content;
        //}
    }
}
