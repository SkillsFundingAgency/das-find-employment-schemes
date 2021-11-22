namespace SFA.DAS.Employer.FrontDoor.Web.Models.Interfaces
{
    public interface IFilterAspect
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

//todo: class will create filterid from guaranteed unique Name (replace spaces)