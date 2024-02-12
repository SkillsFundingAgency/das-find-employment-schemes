using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces;
using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content
{

    [ExcludeFromCodeCoverage]
    public class Content : IContent
    {
        public Content(

            IEnumerable<Page> pages,

            IEnumerable<CaseStudyPage> caseStudyPages,

            IEnumerable<Scheme> schemes,

            Filter motivationsFilter,

            Filter payFilter,

            Filter schemeLengthFilter,

            ContactPage? contactPage,

            InterimLandingPage? landingPage,

            IEnumerable<InterimMenuItem> menuItems,

            IEnumerable<InterimPage> interimPages

        )
        {

            InterimLandingPage = landingPage;

            MenuItems = menuItems;

            Pages = pages;

            CaseStudyPages = caseStudyPages;

            Schemes = schemes;

            MotivationsFilter = motivationsFilter;

            PayFilter = payFilter;

            SchemeLengthFilter = schemeLengthFilter;

            ContactPage = contactPage;

            InterimPages = interimPages;

        }

        public InterimLandingPage? InterimLandingPage { get; }

        public IEnumerable<InterimMenuItem> MenuItems { get; }

        public IEnumerable<Page> Pages { get; }

        public IEnumerable<CaseStudyPage> CaseStudyPages { get; }

        public IEnumerable<Scheme> Schemes { get; }

        public Filter MotivationsFilter { get; }

        public Filter PayFilter { get; }

        public Filter SchemeLengthFilter { get; }

        public ContactPage? ContactPage { get; }

        public IEnumerable<InterimPage> InterimPages { get; }

    }

}
