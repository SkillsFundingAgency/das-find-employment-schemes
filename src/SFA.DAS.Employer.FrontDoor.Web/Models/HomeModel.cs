using System.Collections.Generic;
using System.Reflection.Metadata;

namespace SFA.DAS.Employer.FrontDoor.Web.Models
{
    public class HomeModel
    {
        public IEnumerable<Scheme> Schemes { get; set; }

        public HomeModel()
        {
            Schemes = new[]
            {
                new Scheme("Apprenticeships", "", "", "", "", "apprenticeships")
            };
        }
    }
}
