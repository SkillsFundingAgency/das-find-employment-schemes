using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content;
using SFA.DAS.FindEmploymentSchemes.Contentful.Services.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Web.Models;
using SFA.DAS.FindEmploymentSchemes.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SFA.DAS.FindEmploymentSchemes.Web.Services
{

    public class ContactService : IContactService
    {

        private readonly IContentService _contentService;

        private IReadOnlyList<Contact> _contactModels;

        #pragma warning disable CS8618
        public ContactService(IContentService contentService)
        {

            _contentService = contentService;

            contentService.ContentUpdated += OnContentUpdated;

            BuildModels();

        }

        private void BuildModels()
        {

            _contactModels = BuildContactModelList();

        }

        private void OnContentUpdated(object? sender, EventArgs args)
        {

            BuildModels();

        }

        private ReadOnlyCollection<Contact> BuildContactModelList()
        {

            return _contentService.Content.Contacts.ToList().AsReadOnly();

        }

        public ContactModel GetContactModel()
        {

            return new ContactModel()
            {
                
                ContactList = _contactModels.ToList()
                
            };

        }

    }

}
