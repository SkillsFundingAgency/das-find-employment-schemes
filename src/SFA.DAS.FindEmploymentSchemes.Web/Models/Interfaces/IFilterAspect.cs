namespace SFA.DAS.FindEmploymentSchemes.Web.Models.Interfaces
{
    public interface IFilterAspect
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

//todo: class will create filterid from guaranteed unique Id (replace spaces)