namespace SFA.DAS.FindEmploymentSchemes.Contentful.Model.Content.Interfaces
{
    public interface IFilter
    {
        public string Id { get; set; }
        public string Description { get; set; }
        //todo: doesn't belong here. create new models in web that compose of content filter and selected flag
        public bool Selected { get; set; }
    }
}
