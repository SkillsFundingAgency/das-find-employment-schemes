using Contentful.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public class ContentService : IContentService
    {
        private readonly IContentfulClient _contentfulClient;

        public ContentService(IContentfulClient contentfulClient)
        {
            _contentfulClient = contentfulClient;
        }

        //public async Task Get()
        //{
        //}
    }
}
