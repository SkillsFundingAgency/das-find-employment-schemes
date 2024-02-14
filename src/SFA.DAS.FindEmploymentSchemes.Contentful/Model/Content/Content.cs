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

            List<SchemeFilter> schemeFilters,

            ContactPage? contactPage,

            InterimLandingPage? landingPage,

            IEnumerable<InterimMenuItem> menuItems,

            IEnumerable<InterimPage> interimPages,

            SchemeComparison? schemeComparison,

            BetaBanner? betaBanner

        )
        {

            InterimLandingPage = landingPage;

            MenuItems = menuItems;

            Pages = pages;

            CaseStudyPages = caseStudyPages;

            Schemes = schemes;

            SchemeFilters = schemeFilters;

            ContactPage = contactPage;

            InterimPages = interimPages;

            SchemeComparison = schemeComparison;

            BetaBanner = betaBanner;

        }

        public InterimLandingPage? InterimLandingPage { get; }

        public IEnumerable<InterimMenuItem> MenuItems { get; }

        public IEnumerable<Page> Pages { get; }

        public IEnumerable<CaseStudyPage> CaseStudyPages { get; }

        public IEnumerable<Scheme> Schemes { get; }

        public List<SchemeFilter> SchemeFilters { get; }

        public ContactPage? ContactPage { get; }

        public IEnumerable<InterimPage> InterimPages { get; }

        public SchemeComparison? SchemeComparison { get; }

        public BetaBanner? BetaBanner { get; }

    }

}
