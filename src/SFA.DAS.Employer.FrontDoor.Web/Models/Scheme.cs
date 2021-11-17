using System.Reflection.Metadata;

namespace SFA.DAS.Employer.FrontDoor.Web.Models
{
    //todo: won't end up living here
    public class Scheme
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Document DetailsPageOverride { get; set; }
        public Document Description { get; set; }
    }
}
