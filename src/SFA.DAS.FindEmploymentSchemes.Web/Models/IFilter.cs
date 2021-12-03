namespace SFA.DAS.FindEmploymentSchemes.Web.Models
{
    public interface IFilter
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }
}
