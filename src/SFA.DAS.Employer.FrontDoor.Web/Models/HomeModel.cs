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
                    "Paid employment for over 16's combining work and study in a specific job allowing you to develop your workforce and business.",
                    "Apprentice minimum wage and 5% training contribution depending on business size",
                    "You develop a motivated, skilled and qualified workforce",
                    "Minimum of 12 months employment",
                    "apprenticeships", 1000)
            };
        }
    }
}
