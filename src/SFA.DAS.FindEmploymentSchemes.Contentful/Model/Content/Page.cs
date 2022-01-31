﻿using Microsoft.AspNetCore.Html;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{
    public class Page
    {
        public string Title { get; }
        public string Url { get; }
        public HtmlString Content { get; }

        public Page(string title, string url, HtmlString content)
        {
            Title = title;
            Url = url;
            Content = content;
        }
    }
}