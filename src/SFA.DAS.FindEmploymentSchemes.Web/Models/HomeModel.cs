using System.Collections.Generic;

namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public class HomeModel
    {
        public IEnumerable<Scheme> Schemes { get; set; }

        public HomeModel(IEnumerable<Scheme> schemes)
        {
            Schemes = schemes;
        }
    }
}
