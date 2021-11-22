using NLog.Targets;
using System.Collections;
using System.Collections.Generic;

namespace SFA.DAS.Employer.FrontDoor.Web.Models
{
    public class HomeModel
    {
        public IEnumerable<Scheme> Schemes { get; set; }

        public HomeModel()
        {
            // ensure we order by size desc, so we don't have to sort
            Schemes = new[]
            {
                new Scheme("Apprenticeships",
                    "Paid employment for over 16s combining work and study in a specific job allowing you to develop your workforce and business.",
                    "Apprentice minimum wage and 5% training contribution depending on business size",
                    "You develop a motivated, skilled and qualified workforce",
                    "Minimum of 12 months employment",
                    "apprenticeships", 1000),
                new Scheme("T Levels: industry placements",
                    "Provide a 45 day (315 hours) industry placement for 16 to 19 year-olds, which gives you early access to the brightest entering the market and the opportunity to develop your workforce of the future.",
                    "Free government scheme but you may have your own business costs",
                    "Industry placements can save on many of the costs associated with recruitment",
                    "Short term 45-day industry placement",
                    "t-levels", 900),
                new Scheme("Kickstart Scheme",
                    "Support 16 to 24-year-olds on Universal Credit with a work placement and see if they are a good fit for your business.",
                    "Free government scheme but you may have your own business costs",
                    "Support young people to gain valuable experience and give them opportunities to start a new career in your industry",
                    "6-months",
                    "kickstart-scheme", 800),
                new Scheme("Traineeships",
                    "A 6-week to 12 months skills development programme that includes an unpaid work placement to help 16- to 24-year-olds prepare for an apprenticeship or employment.",
                    "Free government scheme but you may have your own business costs",
                    "Helps you increase your capacity and productivity whilst developing a loyal and talented workforce",
                    "6-week to 12 months",
                    "traineeships", 700)
            };
        }
    }
}
