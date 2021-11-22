using Contentful.Core.Models;

namespace SFA.DAS.Employer.FrontDoor.Contentful.TestHarness.Model
{
    public class Scheme
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Document DetailsPageOverride { get; set; }
        public Document Description { get; set; }
    }
}
