﻿using SFA.DAS.FindEmploymentSchemes.Contentful.Model.Interim;
using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces
{

    public interface IContent
    {

        IEnumerable<Page> Pages { get; }

        IEnumerable<CaseStudyPage> CaseStudyPages { get; }

        /// <summary>
        /// Guaranteed in descending Size order
        /// </summary>
        IEnumerable<Scheme> Schemes { get; }

        List<SchemeFilter> SchemeFilters { get; }

        ContactPage? ContactPage { get; }

        InterimLandingPage? InterimLandingPage { get; }

        IEnumerable<InterimMenuItem> MenuItems { get; }

        IEnumerable<InterimPage> InterimPages { get; }

        SchemeComparison? SchemeComparison { get; }

        BetaBanner? BetaBanner { get; }

        InterimFooterLinks? InterimFooterLinks { get; }

    }

}
