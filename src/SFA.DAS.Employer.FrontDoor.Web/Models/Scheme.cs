
namespace SFA.DAS.Employer.FrontDoor.Web.Models
{
    //todo: won't end up living here
    //todo: revisit for c#9
    public class Scheme
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string? DetailsPageOverride { get; set; }
        public string Description { get; set; }

        public Scheme(string name, string url, string description, string? detailsPageOverride = null)
        {
            Name = name;
            Url = url;
            Description = description;
            DetailsPageOverride = detailsPageOverride;
        }
    }
}
