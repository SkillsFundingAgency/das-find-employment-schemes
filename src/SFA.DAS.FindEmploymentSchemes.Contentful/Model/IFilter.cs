namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model
{
    public interface IFilter
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Order { get; set; }
    }
}
