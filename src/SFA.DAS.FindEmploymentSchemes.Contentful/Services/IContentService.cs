using System;
using System.Threading.Tasks;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Services
{
    public interface IContentService
    {
        event EventHandler<EventArgs>? ContentUpdated;
        IContent Content { get; }
        Task<IContent> Update();
    }
}
