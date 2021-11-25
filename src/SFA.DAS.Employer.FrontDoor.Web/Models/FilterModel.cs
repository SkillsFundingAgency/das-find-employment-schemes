using System.Collections.Generic;

namespace SFA.DAS.Employer.FrontDoor.Web.Models
{
    public class FilterModel
    {
        public IEnumerable<Scheme> Schemes { get; set; }

        public FilterModel(IEnumerable<Scheme> schemes)
        {
            Schemes = schemes;
        }
    }
}
