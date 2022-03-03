
//using Microsoft.AspNetCore.Html;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;


//namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
//{
//    [ExcludeFromCodeCoverage]
//    public class PagePreview : Page
//    {
//        public IEnumerable<PreviewContentError> Errors { get; }

//        public PagePreview(string title, string url, HtmlString content, IEnumerable<PreviewContentError> errors)
//                    : base(title, url, content)
//        {
//            Errors = errors;
//        }

//        public PagePreview(Page page)
//                    : base(page.Title, page.Url, page.Content)
//        {
//            Errors = Enumerable.Empty<PreviewContentError>();
//        }

//        public PagePreview(Page page, IEnumerable<PreviewContentError> errors)
//                    : base(page.Title, page.Url, page.Content)
//        {
//            Errors = errors;
//        }
//    }
//}
